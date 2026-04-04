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
        e.SetEventStatus(EventStatus.Active);
        e.SetMaxGuests(10);
        return e;
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
        e.SetMaxGuests(1);

        var guest1 = CreateGuest("abc");
        var guest2 = CreateGuest("def");

        e.InviteGuest(guest1.email);
        e.InviteGuest(guest2.email);

        e.AcceptInvitation(guest1.email);

        var result = e.AcceptInvitation(guest2.email);

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
}