using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests;

public class DeclineInvitationTests
{
    private static Guest CreateGuest(string email = "abc")
        => ((Success<Guest>)Guest.Create($"{email}@via.dk", "John", "Doe")).Value;

    private static EventRoot CreateActiveEvent()
    {
        var e = EventRoot.Create();
        e.SetEventStatus(EventStatus.Active);
        return e;
    }

    // S1
    [Fact]
    public void DeclineInvitation_Pending_ShouldSucceed()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.InviteGuest(guest.email);

        var result = e.DeclineInvitation(guest.email);

        Assert.True(result.IsSuccess);
        Assert.True(e.IsInvitationDeclined(guest.email));
    }

    // S2
    [Fact]
    public void DeclineInvitation_Accepted_ShouldSucceed()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.InviteGuest(guest.email);
        e.AcceptInvitation(guest.email);

        var result = e.DeclineInvitation(guest.email);

        Assert.True(result.IsSuccess);
        Assert.True(e.IsInvitationDeclined(guest.email));
    }

    // F1
    [Fact]
    public void DeclineInvitation_NotInvited_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        var result = e.DeclineInvitation(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("INVITATION_NOT_FOUND", result.Error.Code);
    }

    // F2
    [Fact]
    public void DeclineInvitation_EventCancelled_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateActiveEvent();

        e.InviteGuest(guest.email);
        e.SetEventStatus(EventStatus.Cancelled);

        var result = e.DeclineInvitation(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_CANCELLED", result.Error.Code);
    }
}