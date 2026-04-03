using UnitTests.Features.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests.Participation;

public class GuestParticipatesPublicEventTests
{
    // S1
    [Fact]
    public void Participate_PublicActiveEventWithSpace_GuestIsAdded()
    {
        var guest = ((Success<Guest>)GuestFactory.Init().Build()).Value;

        var @event = EventFactory.Init()
            .WithPublicVisibility()
            .WithStatus(EventStatus.Active)
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(10)
            .Build();

        var result = @event.AddParticipant(guest.email);

        Assert.True(result.IsSuccess);
    }

    // F1 - Not Active
    [Fact]
    public void Participate_EventNotActive_FailureReturned()
    {
        var guest = ((Success<Guest>)GuestFactory.Init().Build()).Value;

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        var result = @event.AddParticipant(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_NOT_ACTIVE", result.Error.Code);
    }

    // F2 - Full
    [Fact]
    public void Participate_EventIsFull_FailureReturned()
    {
        var guest = ((Success<Guest>)GuestFactory.Init().Build()).Value;

        var @event = EventFactory.Init()
            .WithPublicVisibility()
            .WithStatus(EventStatus.Active)
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(1)
            .Build();

        // Fill event
        var guest2 = ((Success<Guest>)GuestFactory.Init()
            .WithInvalidEmail("abcd@via.dk")
            .Build()).Value;

        @event.AddParticipant(guest2.email);

        var result = @event.AddParticipant(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_IS_FULL", result.Error.Code);
    }

    // F3 - Started
    [Fact]
    public void Participate_EventAlreadyStarted_FailureReturned()
    {
        var guest = ((Success<Guest>)GuestFactory.Init().Build()).Value;

        var @event = EventFactory.Init()
            .WithPublicVisibility()
            .WithStatus(EventStatus.Active)
            .WithValidTimeInPast()
            .Build();

        var result = @event.AddParticipant(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_HAS_STARTED", result.Error.Code);
    }

    // F4 - Private
    [Fact]
    public void Participate_EventIsPrivate_FailureReturned()
    {
        var guest = ((Success<Guest>)GuestFactory.Init().Build()).Value;

        var @event = EventFactory.Init()
            .WithPrivateVisibility()
            .WithStatus(EventStatus.Active)
            .WithValidTimeInFuture()
            .Build();

        var result = @event.AddParticipant(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("EVENT_IS_PRIVATE", result.Error.Code);
    }

    // F5 - Already participating
    [Fact]
    public void Participate_GuestAlreadyJoined_FailureReturned()
    {
        var guest = ((Success<Guest>)GuestFactory.Init().Build()).Value;

        var @event = EventFactory.Init()
            .WithPublicVisibility()
            .WithStatus(EventStatus.Active)
            .WithValidTimeInFuture()
            .Build();

        @event.AddParticipant(guest.email);

        var result = @event.AddParticipant(guest.email);

        Assert.True(result.IsFailure);
        Assert.Equal("GUEST_ALREADY_PARTICIPATING", result.Error.Code);
    }
}