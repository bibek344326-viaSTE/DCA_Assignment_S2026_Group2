using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class UpdateTimeCommand: Command<EventId>
{
    public EventId EventId { get; init; }

    public DateTime StartDateTime { get; init; }
    public DateTime EndDateTime { get; init; }

    private UpdateTimeCommand(EventId eventId, DateTime startDateTime, DateTime endDateTime) : base(eventId)
    {
        EventId = eventId;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
    }

    public static Result<UpdateTimeCommand> Create(Guid eventId, DateTime startDateTime, DateTime endDateTime)
    {
        Result<EventId> idResult = EventId.Create(eventId);

        Result<(DateTime Start, DateTime End)> dateTimeResult = startDateTime <= endDateTime
            ? Result.Success((startDateTime, endDateTime))
            : Result.Failure<(DateTime, DateTime)>(Error.InvalidDateTimeRange);

        return idResult.Combine(dateTimeResult)
            .WithPayloadIfSuccess(() => new UpdateTimeCommand(idResult.Payload!, startDateTime, endDateTime));
    }
}