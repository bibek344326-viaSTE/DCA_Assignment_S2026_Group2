using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class UpdateTitleCommand : Command<EventId>
{
    public EventId EventId { get; init; }
    public string title { get; init; }

    private UpdateTitleCommand(EventId eventId, string title) : base(eventId)
    {
        EventId = eventId;
        this.title = title;
    }

    public static Result<UpdateTitleCommand> Create(Guid eventId, string title)
    {
        Result<EventId> idResult = EventId.Create(eventId);
        Result<string> titleResult = string.IsNullOrWhiteSpace(title)
            ? Result.Failure<string>(Error.BlankString)
            : title.Length < 3
                ? Result.Failure<string>(Error.TooShortTitle(3))
                : title.Length > 75
                    ? Result.Failure<string>(Error.TooLongTitle(75))
                    : Result.Success(title);

        return idResult.Combine(titleResult)
            .WithPayloadIfSuccess(() => new UpdateTitleCommand(idResult.Payload!, titleResult.Payload!));
    }
}
