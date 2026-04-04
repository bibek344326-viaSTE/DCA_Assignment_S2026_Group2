using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class SetMaxGuestsCommand : Command<EventId>
{
    public EventId EventId { get; init; }
    public int MaxGuests { get; init; }

    private SetMaxGuestsCommand(EventId eventId, int maxGuests) : base(eventId)
    {
        EventId = eventId;
        MaxGuests = maxGuests;
    }

    public static Result<SetMaxGuestsCommand> Create(Guid eventId, int maxGuests)
    {
        Result<EventId> idResult = EventId.Create(eventId);

        Result<int> maxGuestsResult = maxGuests < 5
            ? Result.Failure<int>(Error.TooFewGuests(5))
            : maxGuests > 50
                ? Result.Failure<int>(Error.TooManyGuests(50))
                : Result.Success(maxGuests);

        return idResult.Combine(maxGuestsResult)
            .WithPayloadIfSuccess(() => new SetMaxGuestsCommand(idResult.Payload!, maxGuestsResult.Payload!));
    }
}