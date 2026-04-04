using UnitTests.Fakes;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.CreateEvent;

public class CreateEventCommandHandlerTests
{
    private readonly FakeEventRepository _eventRepository = new();
    private readonly FakeUnitOfWork _unitOfWork = new();

    [Fact]
    public async Task CreateEventHandler_WithId_StatusDraftAndMaxGuests5_CreatesAnEmptyEvent()
    {
        // Arrange
        Result<CreateEventCommand> commandResult = CreateEventCommand.Create();
        CreateEventCommand createEventCommand = commandResult.Payload!;

        var handler = new CreateEventCommandHandler(_eventRepository, _unitOfWork);

        // Act
        Result result = await handler.HandleAsync(createEventCommand);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Single(_eventRepository.Values);
        Assert.Equal(1, _unitOfWork.SaveChangesCallCount);
    }
}