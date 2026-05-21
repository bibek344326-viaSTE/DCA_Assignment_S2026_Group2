using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;
using ViaEventAssociation.Infrastructure.EfcQueries.Seed;

namespace IntegrationTests.Queries;

public class QuerySeedTests
{
    [Fact]
    public async Task SeedFromJsonDirectoryAsync_AddsAssignmentEightData()
    {
        if (!QueryTestData.TryGetSeedDirectory(out var seedDirectory))
            Assert.Skip("Assignment 8 seed data directory is not available.");

        var cancellationToken = TestContext.Current.CancellationToken;
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync(cancellationToken);

        var options = new DbContextOptionsBuilder<QueryDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new QueryDbContext(options);
        await context.Database.EnsureCreatedAsync(cancellationToken);

        await context.SeedFromJsonDirectoryAsync(seedDirectory);

        Assert.Equal(28, await context.Events.CountAsync(cancellationToken));
        Assert.Equal(50, await context.Guests.CountAsync(cancellationToken));
        Assert.Equal(9, await context.Locations.CountAsync(cancellationToken));
        Assert.Equal(199, await context.Participations.CountAsync(cancellationToken));
        Assert.Equal(149, await context.Invitations.CountAsync(cancellationToken));
        Assert.Equal(94, await context.JoinRequests.CountAsync(cancellationToken));
    }
}
