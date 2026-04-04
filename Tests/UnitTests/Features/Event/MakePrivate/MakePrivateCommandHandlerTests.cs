using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateCommandHandlerTests
{
    [Fact]
    public async Task MakePrivate_WithValidId_Success()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithPrivateVisibility()
            .Build();

        await repo.AddAsync(@event);

        var command = MakeEventPrivateCommand.Create(@event.Id.Value).Payload!;
        var handler = new MakeEventPrivateCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Equal(1, uow.SaveChangesCallCount);
        Assert.False(@event.IsPublic);
        Assert.Equal(EventStatus.Draft, @event.Status);
    }

    [Fact]
    public async Task MakePrivate_WithEventActive_Failure()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithPrivateVisibility()
            .Build();

        await repo.AddAsync(@event);

        var command = MakeEventPrivateCommand.Create(@event.Id.Value).Payload!;
        var handler = new MakeEventPrivateCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Failure<None>);
        Assert.Equal(0, uow.SaveChangesCallCount);
        Assert.Equal(EventStatus.Active, @event.Status);
    }
}