using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;

namespace UnitTests.Features.Event.CreateEvent;

public class CreateEventCommandTests
{
    [Fact]
    public void CreateEmptyEvent_WithId_StatusDraftAndMaxGuests5_Success()
    {
        Result<CreateEventCommand> result = CreateEventCommand.Create();
        CreateEventCommand command = result.Payload;
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString());
    }
    
}