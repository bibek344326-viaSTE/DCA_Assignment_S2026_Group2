using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests;

public class InviteGuestTests
{
    private static Guest CreateGuest(string email = "abc")
        => ((Success<Guest>)Guest.Create($"{email}@via.dk", "John", "Doe")).Value;

    private static EventRoot CreateEvent(EventStatus status)
    {
        var e = EventRoot.Create();
        var start = DateTime.UtcNow.AddDays(1).Date.AddHours(10);
        e.UpdateDateTime(start, start.AddHours(2));
        e.MakePublic();
        e.SetEventStatus(status);
        e.SetMaxGuests(10);
        return e;
    }

    // S1
    [Theory]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void InviteGuest_EventReadyOrActive_ShouldSucceed(EventStatus status)
    {
        var guest = CreateGuest();
        var e = CreateEvent(status);

        var result = e.InviteGuest(guest.email);

        Assert.True(result.IsSuccess);
        Assert.True(e.HasPendingInvitation(guest.email));
    }

    // F1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Cancelled)]
    public void InviteGuest_EventNotReadyOrActive_ShouldFail(EventStatus status)
    {
        var guest = CreateGuest();
        var e = CreateEvent(status);

        var result = e.InviteGuest(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_NOT_READY_OR_ACTIVE", result.Error.Code);
    }

    // F2
    [Fact]
    public void InviteGuest_EventFull_ShouldFail()
    {
        var e = CreateEvent(EventStatus.Active);

        for (var i = 0; i < 10; i++)
        {
            var participant = CreateGuest($"{i:000000}");
            e.AddParticipant(participant.email);
        }

        var guest2 = CreateGuest("def");

        var result = e.InviteGuest(guest2.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_IS_FULL", result.Error.Code);
    }

    // F3
    [Fact]
    public void InviteGuest_AlreadyInvited_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateEvent(EventStatus.Active);

        e.InviteGuest(guest.email);
        var result = e.InviteGuest(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("GUEST_ALREADY_INVITED", result.Error.Code);
    }

    // F4
    [Fact]
    public void InviteGuest_AlreadyParticipating_ShouldFail()
    {
        var guest = CreateGuest();
        var e = CreateEvent(EventStatus.Active);

        e.AddParticipant(guest.email);

        var result = e.InviteGuest(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("GUEST_ALREADY_PARTICIPATING", result.Error.Code);
    }
}
