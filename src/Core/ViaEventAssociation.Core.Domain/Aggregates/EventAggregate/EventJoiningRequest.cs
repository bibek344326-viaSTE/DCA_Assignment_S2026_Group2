using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventJoiningRequest : Entity<RequestId>
{
    internal Email GuestEmail { get; private set; }
    internal string DescriptionOfJoining { get; private set; }
    internal DateTime RequestDate { get; private set; }
    internal ApprovalStatus ApprovalStatus { get; private set; }

    private EventJoiningRequest(RequestId id, Email guestEmail, string descriptionOfJoining, DateTime requestDate) : base(id)
    {
        GuestEmail = guestEmail;
        DescriptionOfJoining = descriptionOfJoining;
        RequestDate = requestDate;
        ApprovalStatus = ApprovalStatus.Pending;
    }

    public static Result<EventJoiningRequest> Create(Email guestEmail, string descriptionOfJoining, DateTime? requestDate = null)
        => Result.Success(new EventJoiningRequest(RequestId.Create(), guestEmail, descriptionOfJoining, requestDate ?? DateTime.UtcNow));

    public Result<None> Approve()
    {
        if (ApprovalStatus is ApprovalStatus.Approved)
            return Result.Success();

        ApprovalStatus = ApprovalStatus.Approved;
        return Result.Success();
    }

    public Result<None> Reject()
    {
        if (ApprovalStatus is ApprovalStatus.Rejected)
            return Result.Success();

        ApprovalStatus = ApprovalStatus.Rejected;
        return Result.Success();
    }
}
