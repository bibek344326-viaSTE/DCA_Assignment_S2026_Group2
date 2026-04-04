using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuestsCommandHandlerTests
{
    // S1
    [Theory]
    [InlineData(5)]
    public async Task SetMaxGuests_WithValidMaxGuestsValues_Success(int maxGuests)
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var command = SetMaxGuestsCommand.Create(@event.Id.Value, maxGuests).Payload!;
        var handler = new SetMaxGuestCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    // F4
    [Fact]
    public void SetMaxGuests_WithTooFewGuests_Failure()
    {
        // Arrange
        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();

        // Act
        Result<SetMaxGuestsCommand> commandResult = SetMaxGuestsCommand.Create(@event.Id.Value, 4);

        // Assert
        Assert.True(commandResult.IsFailure);
        Assert.Null(commandResult.Payload);
    }
}