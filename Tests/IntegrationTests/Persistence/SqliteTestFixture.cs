using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

namespace IntegrationTests.Persistence;

public sealed class SqliteTestFixture : IDisposable
{
    private readonly SqliteConnection _connection;

    public DmContext Context { get; }

    public SqliteTestFixture()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<DmContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new DmContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
