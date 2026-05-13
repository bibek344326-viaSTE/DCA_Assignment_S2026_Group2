using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Guest;

public class CancelParticipationCommand : Command<EventId>
{
    public Email GuestEmail { get; }

    private CancelParticipationCommand(EventId eventId, Email guestEmail) : base(eventId)
    {
        GuestEmail = guestEmail;
    }

    public static Result<CancelParticipationCommand> Create(Guid eventId, string? guestEmail)
    {
        var idResult = EventId.Create(eventId);
        var emailResult = Email.Create(guestEmail);

        return idResult
            .Combine(emailResult)
            .WithPayloadIfSuccess(() => new CancelParticipationCommand(idResult.Payload!, emailResult.Payload!));
    }
}
