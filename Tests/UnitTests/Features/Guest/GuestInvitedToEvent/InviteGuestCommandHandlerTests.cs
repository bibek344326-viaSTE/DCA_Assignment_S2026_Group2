using UnitTests.Fakes;
using UnitTests.Features.Event;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Application.CommandHandlers.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Features.Guests;

public class InviteGuestCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ReadyEvent_AddsPendingInvitation_AndSaves()
    {
        var eventRepository = new FakeEventRepository();
        var guestRepository = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var guest = GuestFactory.Init().Build().Payload!;
        await guestRepository.AddAsync(guest);

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Ready)
            .Build();
        await eventRepository.AddAsync(@event);

        var command = InviteGuestCommand.Create(@event.Id.Value, guest.Id.Value).Payload!;
        var handler = new InviteGuestCommandHandler(eventRepository, guestRepository, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.True(@event.HasPendingInvitation(guest.Id));
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_GuestNotFound_ReturnsFailure_AndDoesNotSave()
    {
        var eventRepository = new FakeEventRepository();
        var uow = new FakeUnitOfWork();
        var @event = EventFactory.Init().WithStatus(EventStatus.Ready).Build();
        await eventRepository.AddAsync(@event);

        var command = InviteGuestCommand.Create(@event.Id.Value, "abc@via.dk").Payload!;
        var handler = new InviteGuestCommandHandler(eventRepository, new FakeGuestRepository(), uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        Assert.Equal(0, uow.SaveChangesCallCount);
    }
}
