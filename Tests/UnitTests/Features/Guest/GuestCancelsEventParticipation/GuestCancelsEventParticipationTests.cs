using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests;

public class CancelParticipationTests
{
    private static Guest CreateGuest(string email = "abc")
        => ((Success<Guest>)Guest.Create($"{email}@via.dk", "John", "Doe")).Value;

    private static EventRoot CreateActiveEvent()
    {
        var e = EventRoot.Create();

        var updateTitleResult = e.UpdateTitle("Test Event");
        Assert.True(updateTitleResult.IsSuccess);

        var updateDescriptionResult = e.UpdateDescription("Description");
        Assert.True(updateDescriptionResult.IsSuccess);

        var eventStart = DateTime.Today.AddDays(1).AddHours(9);
        var updateDateTimeResult = e.UpdateDateTime(eventStart, eventStart.AddHours(2));
        Assert.True(updateDateTimeResult.IsSuccess);

        e.MakePublic();
        e.SetMaxGuests(10);

        var readyResult = e.Ready();
        Assert.True(readyResult.IsSuccess);

        var activateResult = e.Activate();
        Assert.True(activateResult.IsSuccess);
        return e;
    }

    // S1
    [Fact]
    public void CancelParticipation_GuestIsParticipating_ShouldRemoveGuest()
    {
        // Arrange
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.AddParticipant(guest.email);

        // Act
        var result = e.RemoveParticipant(guest.email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(e.IsParticipant(guest.email));
    }

    // S2
    [Fact]
    public void CancelParticipation_GuestNotParticipating_ShouldDoNothing()
    {
        // Arrange
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        // Act
        var result = e.RemoveParticipant(guest.email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(e.IsParticipant(guest.email));
    }

    // F1
    [Fact]
    public void CancelParticipation_EventStarted_ShouldFail()
    {
        // Arrange
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.AddParticipant(guest.email);
        var startProperty = typeof(EventRoot).GetProperty("EventStartDateTime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        startProperty!.SetValue(e, DateTime.UtcNow.AddHours(-1));

        // Act
        var result = e.RemoveParticipant(guest.email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_HAS_STARTED", result.Error.Code);
    }
}
