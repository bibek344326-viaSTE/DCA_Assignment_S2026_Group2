using ViaEventAssociation.Core.QueryContracts.Queries;

namespace IntegrationTests.Queries;

public class GetEventGuestStatusOverviewQueryTests
{
    [Fact]
    public async Task DispatchAsync_ReturnsGuestStatusGroupsForEvent()
    {
        if (!QueryTestData.TryGetSeedDirectory(out _))
            Assert.Skip("Assignment 8 seed data directory is not available.");

        await using var fixture = await QueryTestFixture.CreateSeededAsync(new DateTime(2024, 3, 15, 0, 0, 0));
        var eventId = Guid.Parse("27a5bde5-3900-4c45-9358-3d186ad6b2d7");

        var answer = await fixture.Dispatcher().DispatchAsync(new GetEventGuestStatusOverviewQuery(eventId));

        Assert.NotNull(answer);
        Assert.False(string.IsNullOrWhiteSpace(answer.Title));
        Assert.NotEmpty(answer.PendingInvitations);
        Assert.NotEmpty(answer.PendingJoinRequests);
        Assert.All(answer.PendingJoinRequests, request => Assert.False(string.IsNullOrWhiteSpace(request.Reason)));
    }
}
