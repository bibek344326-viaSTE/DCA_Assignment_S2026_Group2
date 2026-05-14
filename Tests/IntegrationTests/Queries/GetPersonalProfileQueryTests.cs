using ViaEventAssociation.Core.QueryContracts.Queries;

namespace IntegrationTests.Queries;

public class GetPersonalProfileQueryTests
{
    [Fact]
    public async Task DispatchAsync_ReturnsGuestProfileWithUpcomingPastAndPendingInvitationCounts()
    {
        await using var fixture = await QueryTestFixture.CreateSeededAsync(new DateTime(2024, 3, 15, 0, 0, 0));
        var guestId = Guid.Parse("230c1a99-d5c7-4fbc-9f48-07ccbb100936");

        var answer = await fixture.Dispatcher().DispatchAsync(new GetPersonalProfileQuery(guestId));

        Assert.NotNull(answer);
        Assert.Equal("John", answer.FirstName);
        Assert.Equal("Doe", answer.LastName);
        Assert.Equal("286848@via.dk", answer.Email);
        Assert.Equal(answer.UpcomingEvents.Count, answer.UpcomingEventCount);
        Assert.True(answer.UpcomingEvents.All(e => e.StartTime > new DateTime(2024, 3, 15, 0, 0, 0)));
        Assert.True(answer.PastEvents.Count <= 5);
        Assert.True(answer.PendingInvitationCount >= 0);
    }
}
