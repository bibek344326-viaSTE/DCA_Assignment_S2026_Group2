using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;
using ViaEventAssociation.Infrastructure.EfcQueries.Extensions;
using ViaEventAssociation.Infrastructure.EfcQueries.Seed;

namespace IntegrationTests.Queries;

internal sealed class QueryTestFixture : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ServiceProvider _provider;

    public IServiceProvider Services => _provider;

    private QueryTestFixture(SqliteConnection connection, ServiceProvider provider)
    {
        _connection = connection;
        _provider = provider;
    }

    public static async Task<QueryTestFixture> CreateSeededAsync(DateTime now)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var services = new ServiceCollection();
        services.AddDbContext<QueryDbContext>(options => options.UseSqlite(connection));
        services.AddScoped<ISystemTime>(_ => new FakeSystemTime(now));
        services.AddEfcQueries();

        var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<QueryDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.SeedFromJsonDirectoryAsync(QueryTestData.SeedDirectory);

        return new QueryTestFixture(connection, provider);
    }

    public IQueryDispatcher Dispatcher()
        => _provider.GetRequiredService<IQueryDispatcher>();

    public QueryDbContext Context()
        => _provider.GetRequiredService<QueryDbContext>();

    public async ValueTask DisposeAsync()
    {
        await _provider.DisposeAsync();
        await _connection.DisposeAsync();
    }

    private sealed class FakeSystemTime(DateTime now) : ISystemTime
    {
        public DateTime Now { get; } = now;
    }
}
