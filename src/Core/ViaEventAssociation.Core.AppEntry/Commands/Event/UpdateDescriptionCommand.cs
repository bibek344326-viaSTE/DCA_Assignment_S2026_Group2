using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class UpdateDescriptionCommand : Command<EventId>
{
    public EventId EventId { get; init; }
    public string Description { get; init; }

    private UpdateDescriptionCommand(EventId eventId, string description) : base(eventId)
    {
        EventId = eventId;
        Description = description;
    }

    public static Result<UpdateDescriptionCommand> Create(Guid eventId, string? description)
    {
        Result<EventId> idResult = EventId.Create(eventId);

        Result<string> descriptionResult = description is null || description.Length == 0
            ? Result.Success(string.Empty)
            : string.IsNullOrWhiteSpace(description)
                ? Result.Failure<string>(Error.BlankString)
                : description.Length > 250
                    ? Result.Failure<string>(Error.TooLongDescription(250))
                    : Result.Success(description);

        return idResult.Combine(descriptionResult)
            .WithPayloadIfSuccess(() => new UpdateDescriptionCommand(idResult.Payload!, descriptionResult.Payload!));
    }
}