using ViaEventAssociation.Core.QueryContracts.Queries;

namespace IntegrationTests.Queries;

public class GetSingleEventQueryTests
{
    [Fact]
    public async Task DispatchAsync_ReturnsEventDetailsAndPagedGuests()
    {
        if (!QueryTestData.TryGetSeedDirectory(out _))
            Assert.Skip("Assignment 8 seed data directory is not available.");

        await using var fixture = await QueryTestFixture.CreateSeededAsync(new DateTime(2024, 3, 15, 0, 0, 0));
        var eventId = Guid.Parse("40ed2fd9-2240-4791-895f-b9da1a1f64e4");

        var answer = await fixture.Dispatcher().DispatchAsync(new GetSingleEventQuery(eventId, GuestOffset: 0, GuestPageSize: 9));

        Assert.NotNull(answer);
        Assert.Equal("Friday Bar", answer.Title);
        Assert.Equal("public", answer.Visibility);
        Assert.Equal(9, answer.Guests.Count);
        Assert.True(answer.TotalGuests >= answer.Guests.Count);
        Assert.Equal(answer.TotalGuests, answer.AttendeeCount);
        Assert.False(string.IsNullOrWhiteSpace(answer.LocationName));
    }
}
