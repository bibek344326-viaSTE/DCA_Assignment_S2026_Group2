using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_EventExistsAndTitleValid_ReturnsSuccess_AndSaves()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var commandResult = UpdateTitleCommand.Create(@event.Id.Value, "Updated Title");
        var command = commandResult.Payload!;

        var handler = new UpdateTitleCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_EventExistsButTitleBlank_ReturnsFailure_AndDoesNotSave()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var command = UpdateTitleCommand.Create(@event.Id.Value, "New Title").Payload!;
        @event.SetEventStatus(EventStatus.Active);

        var handler = new UpdateTitleCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Failure<None>);
        Assert.Equal(0, uow.SaveChangesCallCount);
    }
}