using UnitTests.Fakes;
using UnitTests.Features.Event;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Application.CommandHandlers.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Features.Guests;

public class DeclineInvitationCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_PendingInvitation_DeclinesInvitation_AndSaves()
    {
        var eventRepository = new FakeEventRepository();
        var guestRepository = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var guest = GuestFactory.Init().Build().Payload!;
        await guestRepository.AddAsync(guest);

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .Build();
        @event.InviteGuest(guest.Id);
        await eventRepository.AddAsync(@event);

        var command = DeclineInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload!;
        var handler = new DeclineInvitationCommandHandler(eventRepository, guestRepository, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.True(@event.IsInvitationDeclined(guest.Id));
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_NoInvitation_ReturnsFailure_AndDoesNotSave()
    {
        var eventRepository = new FakeEventRepository();
        var guestRepository = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var guest = GuestFactory.Init().Build().Payload!;
        await guestRepository.AddAsync(guest);
        var @event = EventFactory.Init().WithValidTimeInFuture().WithStatus(EventStatus.Active).Build();
        await eventRepository.AddAsync(@event);

        var command = DeclineInvitationCommand.Create(@event.Id.Value, guest.Id.Value).Payload!;
        var handler = new DeclineInvitationCommandHandler(eventRepository, guestRepository, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        Assert.Equal(0, uow.SaveChangesCallCount);
    }
}
