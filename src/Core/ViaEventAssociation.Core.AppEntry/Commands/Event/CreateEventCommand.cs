
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
public class CreateEventCommand : Command<EventId>
{
    /// Initializes a new instance of the CreateEventCommand.
    private CreateEventCommand(EventId eventId) : base(eventId)
    {
    }

    /// Factory method to create a new CreateEventCommand.
    //A Result containing the newly created CreateEventCommand.
    public static Result<CreateEventCommand> Create()
    {
        var eventId = EventId.Create();
        var command = new CreateEventCommand(eventId);
        return Result.Success(command);

    }
}