using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests;

public class AcceptInvitationTests
{
    private static Guest CreateGuest(string email = "abc")
        => ((Success<Guest>)Guest.Create($"{email}@via.dk", "John", "Doe")).Value;

    private static EventRoot CreateActiveEvent()
    {
        var e = EventRoot.Create();
        var start = DateTime.UtcNow.AddDays(1).Date.AddHours(10);
        e.UpdateDateTime(start, start.AddHours(2));
        e.MakePublic();
        e.SetEventStatus(EventStatus.Active);
        e.SetMaxGuests(10);
        return e;
    }

    private static void SetStartTime(EventRoot e, DateTime start)
    {
        var startProperty = typeof(EventRoot).GetProperty("EventStartDateTime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        startProperty!.SetValue(e, start);
    }

    // S1
    [Fact]
    public void AcceptInvitation_Valid_ShouldSucceed()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.InviteGuest(guest.email);

        var result = e.AcceptInvitation(guest.email);

        Assert.True(result.IsSuccess);
        Assert.True(e.IsParticipant(guest.email));
    }

    // F1
    [Fact]
    public void AcceptInvitation_NotInvited_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        var result = e.AcceptInvitation(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("INVITATION_NOT_FOUND", result.Error.Code);
    }

    // F2
    [Fact]
    public void AcceptInvitation_EventFull_ShouldFail()
    {
        var e = CreateActiveEvent();

        for (var i = 0; i < 11; i++)
        {
            e.InviteGuest(CreateGuest($"{i:000000}").email);
        }

        for (var i = 0; i < 10; i++)
        {
            e.AcceptInvitation(CreateGuest($"{i:000000}").email);
        }

        var result = e.AcceptInvitation(CreateGuest("000010").email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_IS_FULL", result.Error.Code);
    }

    // F3
    [Fact]
    public void AcceptInvitation_EventCancelled_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.InviteGuest(guest.email);
        e.SetEventStatus(EventStatus.Cancelled);

        var result = e.AcceptInvitation(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_CANCELLED", result.Error.Code);
    }

    // F4
    [Fact]
    public void AcceptInvitation_EventReady_ShouldFail()
    {
        var guest = CreateGuest();
        var e = EventRoot.Create();
        e.SetEventStatus(EventStatus.Ready);

        e.InviteGuest(guest.email);

        var result = e.AcceptInvitation(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_NOT_ACTIVE", result.Error.Code);
    }

    // F5
    [Fact]
    public void AcceptInvitation_EventInPast_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.InviteGuest(guest.email);
        SetStartTime(e, DateTime.UtcNow.AddHours(-1));

        var result = e.AcceptInvitation(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_HAS_STARTED", result.Error.Code);
    }
}
