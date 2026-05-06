using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.ActivatesEvent;

public class ActivateEventCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DraftEventWithValidData_ActivatesEvent_AndSaves()
    {
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithPublicVisibility()
            .WithMaxNumberOfGuests(10)
            .Build();

        await repo.AddAsync(@event);

        var command = ActivateEventCommand.Create(@event.Id.Value).Payload!;
        var handler = new ActivateEventCommandHandler(repo, uow);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
        Assert.Equal(1, uow.SaveChangesCallCount);
    }
}
