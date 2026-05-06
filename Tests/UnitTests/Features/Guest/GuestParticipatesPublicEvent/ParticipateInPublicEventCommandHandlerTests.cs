using UnitTests.Fakes;
using UnitTests.Features.Event;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Application.CommandHandlers.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests;

public class ParticipateInPublicEventCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_ActivePublicEventWithSpace_AddsParticipant_AndSaves()
    {
        var eventRepository = new FakeEventRepository();
        var guestRepository = new FakeGuestRepository();
        var uow = new FakeUnitOfWork();

        var guest = GuestFactory.Init().Build().Payload!;
        await guestRepository.AddAsync(guest);

        var @event = EventFactory.Init()
            .WithPublicVisibility()
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(10)
            .WithStatus(EventStatus.Active)
            .Build();
        await eventRepository.AddAsync(@event);

        var command = ParticipateInPublicEventCommand.Create(@event.Id.Value, guest.Id.Value).Payload!;
        var handler = new ParticipateInPublicEventCommandHandler(eventRepository, guestRepository, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.True(@event.IsParticipant(guest.Id));
        Assert.Equal(1, uow.SaveChangesCallCount);
    }
}
