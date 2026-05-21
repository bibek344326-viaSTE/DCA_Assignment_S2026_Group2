using ViaEventAssociation.Core.QueryContracts.Queries;

namespace IntegrationTests.Queries;

public class GetEventEditingOverviewQueryTests
{
    [Fact]
    public async Task DispatchAsync_ReturnsSeparateDraftReadyAndCancelledLists()
    {
        if (!QueryTestData.TryGetSeedDirectory(out _))
            Assert.Skip("Assignment 8 seed data directory is not available.");

        await using var fixture = await QueryTestFixture.CreateSeededAsync(new DateTime(2024, 3, 15, 0, 0, 0));

        var answer = await fixture.Dispatcher().DispatchAsync(new GetEventEditingOverviewQuery());

        Assert.NotNull(answer.Drafts);
        Assert.NotNull(answer.Readied);
        Assert.NotNull(answer.Cancelled);
        Assert.DoesNotContain(answer.Drafts, item => answer.Readied.Any(ready => ready.EventId == item.EventId));
        Assert.DoesNotContain(answer.Drafts, item => answer.Cancelled.Any(cancelled => cancelled.EventId == item.EventId));
    }
}
