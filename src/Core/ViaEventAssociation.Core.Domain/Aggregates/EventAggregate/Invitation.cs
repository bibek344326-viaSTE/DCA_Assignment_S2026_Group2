using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class Invitation : Entity<InvitationId>
{
    internal Email GuestEmail { get; private set; }
    internal DateTime SentDate { get; private set; }
    internal InvitationStatus InvitationStatus { get; private set; }

    private Invitation(InvitationId id, Email guestEmail, DateTime sentDate) : base(id)
    {
        GuestEmail = guestEmail;
        SentDate = sentDate;
        InvitationStatus = InvitationStatus.Pending;
    }

    public static Result<Invitation> Create(Email guestEmail, DateTime? sentDate = null)
        => Result.Success(new Invitation(InvitationId.Create(), guestEmail, sentDate ?? DateTime.UtcNow));

    public Result<None> Accept()
    {
        if (InvitationStatus is InvitationStatus.Accepted)
            return Result.Success();

        if (InvitationStatus is InvitationStatus.Declined)
            return Error.InvitationNotFound;

        InvitationStatus = InvitationStatus.Accepted;
        return Result.Success();
    }

    public Result<None> Reject()
    {
        if (InvitationStatus is InvitationStatus.Declined)
            return Result.Success();

        InvitationStatus = InvitationStatus.Declined;
        return Result.Success();
    }
}
