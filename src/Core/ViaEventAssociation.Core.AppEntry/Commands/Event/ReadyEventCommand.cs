using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ReadyEventCommand : Command<EventId>
{
    private ReadyEventCommand(EventId eventId) : base(eventId)
    {
    }

    public static Result<ReadyEventCommand> Create(Guid eventId)
    {
        var idResult = EventId.Create(eventId);
        return idResult.WithPayloadIfSuccess(() => new ReadyEventCommand(idResult.Payload!));
    }
}
