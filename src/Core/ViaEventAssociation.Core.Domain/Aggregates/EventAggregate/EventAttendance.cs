using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventAttendance : Entity<AttendanceId>
{
    internal Email GuestEmail { get; private set; }
    internal DateTime RegisteredDate { get; private set; }
    internal bool IsCancelled { get; private set; }

    private EventAttendance(AttendanceId id, Email guestEmail, DateTime registeredDate) : base(id)
    {
        GuestEmail = guestEmail;
        RegisteredDate = registeredDate;
        IsCancelled = false;
    }

    public static Result<EventAttendance> Create(Email guestEmail, DateTime? registeredDate = null)
        => Result.Success(new EventAttendance(AttendanceId.Create(), guestEmail, registeredDate ?? DateTime.UtcNow));

    public Result<None> Register()
    {
        IsCancelled = false;
        return Result.Success();
    }

    public Result<None> Cancel()
    {
        IsCancelled = true;
        return Result.Success();
    }
}
