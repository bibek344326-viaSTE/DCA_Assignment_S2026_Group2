using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ActivateEventCommand : Command<EventId>
{
    private ActivateEventCommand(EventId eventId) : base(eventId)
    {
    }

    public static Result<ActivateEventCommand> Create(Guid eventId)
    {
        var idResult = EventId.Create(eventId);
        return idResult.WithPayloadIfSuccess(() => new ActivateEventCommand(idResult.Payload!));
    }
}
