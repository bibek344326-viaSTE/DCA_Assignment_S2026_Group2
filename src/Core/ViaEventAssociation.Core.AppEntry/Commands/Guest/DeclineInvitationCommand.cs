using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Guest;

public class DeclineInvitationCommand : Command<EventId>
{
    public Email GuestEmail { get; }

    private DeclineInvitationCommand(EventId eventId, Email guestEmail) : base(eventId)
    {
        GuestEmail = guestEmail;
    }

    public static Result<DeclineInvitationCommand> Create(Guid eventId, string? guestEmail)
    {
        var idResult = EventId.Create(eventId);
        var emailResult = Email.Create(guestEmail);

        return idResult
            .Combine(emailResult)
            .WithPayloadIfSuccess(() => new DeclineInvitationCommand(idResult.Payload!, emailResult.Payload!));
    }
}
