using UnitTests.Fakes;
using UnitTests.Features.Event;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Application.CommandHandlers.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Features.Guests;

public class CancelParticipationCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ParticipatingGuest_RemovesParticipant_AndSaves()
    {
        var eventRepository = new FakeEventRepository();
        var guestRepository = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var guest = GuestFactory.Init().Build().Payload!;
        await guestRepository.AddAsync(guest);

        var @event = EventFactory.Init()
            .WithPublicVisibility()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .Build();
        @event.AddParticipant(guest.Id);
        await eventRepository.AddAsync(@event);

        var command = CancelParticipationCommand.Create(@event.Id.Value, guest.Id.Value).Payload!;
        var handler = new CancelParticipationCommandHandler(eventRepository, guestRepository, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.False(@event.IsParticipant(guest.Id));
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_EventNotFound_ReturnsFailure_AndDoesNotSave()
    {
        var handler = new CancelParticipationCommandHandler(new FakeEventRepository(), new FakeGuestRepository(), new FakeUnitOfWork());
        var command = CancelParticipationCommand.Create(Guid.NewGuid(), "abc@via.dk").Payload!;

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}
