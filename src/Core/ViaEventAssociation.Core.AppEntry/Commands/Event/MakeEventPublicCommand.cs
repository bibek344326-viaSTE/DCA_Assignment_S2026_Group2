using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class MakeEventPublicCommand : Command<EventId>
{
    public EventId EventId { get; init; }

    private MakeEventPublicCommand(EventId eventId) : base(eventId)
    {
        EventId = eventId;
    }

    public static Result<MakeEventPublicCommand> Create(Guid eventId)
    {
        Result<EventId> idResult = EventId.Create(eventId);

        return idResult.WithPayloadIfSuccess(() => new MakeEventPublicCommand(idResult.Payload!));
    }
}