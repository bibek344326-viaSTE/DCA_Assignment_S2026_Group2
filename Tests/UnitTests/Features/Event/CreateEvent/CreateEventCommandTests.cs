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
        
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        var command = result.Payload;
        Assert.NotNull(command.Id);
        Assert.False(string.IsNullOrEmpty(command.Id.ToString()));
    }
    
}
