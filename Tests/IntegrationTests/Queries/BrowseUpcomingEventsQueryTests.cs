using ViaEventAssociation.Core.QueryContracts.Queries;

namespace IntegrationTests.Queries;

public class BrowseUpcomingEventsQueryTests
{
    [Fact]
    public async Task DispatchAsync_ReturnsPagedUpcomingEventsOrderedByStart()
    {
        if (!QueryTestData.TryGetSeedDirectory(out _))
            Assert.Skip("Assignment 8 seed data directory is not available.");

        await using var fixture = await QueryTestFixture.CreateSeededAsync(new DateTime(2024, 3, 15, 0, 0, 0));
        var dispatcher = fixture.Dispatcher();
        var answer = await dispatcher.DispatchAsync(new BrowseUpcomingEventsQuery(null, PageNumber: 1, PageSize: 3));

        Assert.Equal(1, answer.PageNumber);
        Assert.Equal(3, answer.PageSize);
        Assert.Equal(3, answer.Events.Count);
        Assert.True(answer.TotalItems > 3);
        Assert.True(answer.TotalPages > 1);
        Assert.True(answer.Events.SequenceEqual(answer.Events.OrderBy(e => e.Start)));
    }
}
