using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakePublic;

public class MakePublicCommandHandlerTests
{
    [Fact]
    public async Task MakePublic_WithValidId_Success()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).WithPrivateVisibility().Build();
        await repo.AddAsync(@event);

        var command = MakeEventPublicCommand.Create(@event.Id.Value).Payload!;
        var handler = new MakeEventPublicCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Equal(1, uow.SaveChangesCallCount);
        Assert.True(@event.IsPublic);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    [Fact]
    public async Task MakePublic_WithEventCancelled_Failure()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();
        await repo.AddAsync(@event);

        var command = MakeEventPublicCommand.Create(@event.Id.Value).Payload!;
        var handler = new MakeEventPublicCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Failure<None>);
        Assert.Equal(0, uow.SaveChangesCallCount);
        Assert.Equal(EventStatus.Cancelled, @event.Status);
    }
}