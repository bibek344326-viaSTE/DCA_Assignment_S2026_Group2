using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescriptionCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_EventExistsAndDescriptionValid_ReturnsSuccess_AndSaves()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var commandResult = UpdateDescriptionCommand.Create(@event.Id.Value, "New description");
        var command = commandResult.Payload!;

        var handler = new UpdateDescriptionCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_EventExistsButDescriptionTooLong_ReturnsFailure_AndDoesNotSave()
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var tooLongDescription = new string('a', 251);
        var ctor = typeof(UpdateDescriptionCommand)
            .GetConstructor(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                binder: null,
                types: [typeof(EventId), typeof(string)],
                modifiers: null);

        var command = (UpdateDescriptionCommand)ctor!.Invoke([@event.Id, tooLongDescription]);

        var handler = new UpdateDescriptionCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Failure<None>);
        Assert.Equal(0, uow.SaveChangesCallCount);
    }
}