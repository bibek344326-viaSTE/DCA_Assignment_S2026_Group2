using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventRoot : AggregateRoot<EventId>
{
    internal string EventTitle { get; private set; }
    internal string EventDescription { get; private set; }
    internal DateTime EventStartDateTime { get; private set; }
    internal DateTime EventEndDateTime { get; private set; }
    internal bool? IsPublic { get; private set; }
    internal int MaxGuests { get; private set; }
    internal EventStatus EventStatus { get; private set; }

    internal LocationId? LocationId { get; private set; }

    private readonly HashSet<Email> _participants = [];
    private readonly HashSet<Email> _invitations = [];
    private readonly HashSet<Email> _declinedInvitations = [];

    public EventStatus Status => EventStatus;

    private EventRoot(EventId id) : base(id)
    {
        EventTitle = "Working Title";
        EventDescription = string.Empty;
        IsPublic = null;
        MaxGuests = 5;
        EventStatus = EventStatus.Draft;
    }

    public static EventRoot Create() => new(EventId.Create());

    public void SetEventStatus(EventStatus status) => EventStatus = status;

    public Result<None> UpdateTitle(string title)
    {
        if (EventStatus is EventStatus.Active) return Error.EventStatusIsActive;
        if (EventStatus is EventStatus.Cancelled) return Error.EventStatusIsCanceled;

        if (title is null) return Error.NullString;
        if (string.IsNullOrWhiteSpace(title)) return Error.BlankString;
        if (title.Length < 3) return Error.TooShortTitle(3);
        if (title.Length > 75) return Error.TooLongTitle(75);

        EventTitle = title;
        if (EventStatus == EventStatus.Ready) EventStatus = EventStatus.Draft;
        return Result.Success();
    }

    public static EventRoot Create(EventId id)
    {
        return new EventRoot(id);
    }
    
    public Result<None> UpdateDescription(string description)

    {
        if (EventStatus is EventStatus.Active) return Error.EventStatusIsActive;
        if (EventStatus is EventStatus.Cancelled) return Error.EventStatusIsCanceled;

        var d = description ?? "";
        if (d.Length > 250) return Error.TooLongDescription(250);

        EventDescription = d;
        if (EventStatus == EventStatus.Ready) EventStatus = EventStatus.Draft;
        return Result.Success();
    }

    public Result<None> UpdateDateTime(DateTime startTime, DateTime endTime)
    {
        var errors = new HashSet<Error>();

        if (EventStatus is EventStatus.Active) errors.Add(Error.EventStatusIsActive);
        if (EventStatus is EventStatus.Cancelled) errors.Add(Error.EventStatusIsCanceled);

        if (startTime < DateTime.Now) errors.Add(Error.EventStartTimeInThePast);

        if (startTime >= endTime)
        {
            errors.Add(Error.InvalidDateTimeRange);
        }
        else
        {
            var totalHours = (endTime - startTime).TotalHours;
            if (totalHours < 1) errors.Add(Error.DurationTooShort);
            if (totalHours > 10) errors.Add(Error.DurationTooLong);
        }

        var startHour = startTime.TimeOfDay.TotalHours;
        var endHour = endTime.TimeOfDay.TotalHours;

        if ((startHour >= 1 && startHour < 8))
            errors.Add(Error.InvalidStartDateTime);

        if ((endHour > 1 && endHour < 8))
            errors.Add(Error.InvalidEndDateTime);

        if (startTime.Date == endTime.Date && startHour < 1 && endHour >= 8)
            errors.Add(Error.InvalidEndDateTime);

        if (errors.Count != 0) return Result.Failure<None>(errors);

        EventStartDateTime = startTime;
        EventEndDateTime = endTime;
        if (EventStatus == EventStatus.Ready) EventStatus = EventStatus.Draft;
        return Result.Success();
    }

    public Result<None> SetLocation(LocationId locationId)
    {
        if (EventStatus is EventStatus.Active) return Error.EventStatusIsActive;
        if (EventStatus is EventStatus.Cancelled) return Error.EventStatusIsCanceled;

        LocationId = locationId;
        if (EventStatus == EventStatus.Ready) EventStatus = EventStatus.Draft;
        return Result.Success();
    }
    public Result<None> AddParticipant(Email guestEmail)
    {
        if (EventStatus != EventStatus.Active)
            return Error.EventNotActive;

        if (IsPublic != true)
            return Error.EventIsPrivate;

        if (EventStartDateTime <= DateTime.UtcNow)
            return Error.EventHasStarted;

        if (_participants.Contains(guestEmail))
            return Error.GuestAlreadyParticipating;

        if (_participants.Count >= MaxGuests)
            return Error.EventIsFull;

        _participants.Add(guestEmail);
        return Result.Success();
    }

    public bool IsParticipant(Email email) => _participants.Contains(email);

    public int GetCurrentParticipantCount() => _participants.Count;

    public Result<None> Ready()
    {
        var errors = new HashSet<Error>();

        if (EventStatus is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        if (EventTitle == "Working Title")
            errors.Add(Error.EventTitleIsDefault);

        if (string.IsNullOrEmpty(EventDescription))
            errors.Add(Error.EventDescriptionMissing);

        var isDateTimeMissing = EventStartDateTime == DateTime.MinValue || EventEndDateTime == DateTime.MinValue;
        if (isDateTimeMissing)
            errors.Add(Error.EventDateTimeMissing);

        if (!isDateTimeMissing && EventStartDateTime < DateTime.Now)
            errors.Add(Error.EventStartTimeInThePast);

        if (IsPublic is null)
            errors.Add(Error.EventVisibilityMissing);

        if (MaxGuests < 5 || MaxGuests > 50)
            errors.Add(Error.InvalidMaxGuestsRange);

        if (errors.Count != 0)
            return Result.Failure<None>(errors);

        EventStatus = EventStatus.Ready;
        return Result.Success();
    }

    public Result<None> Activate()
    {
        if (EventStatus is EventStatus.Cancelled)
            return Error.EventStatusIsCanceled;

        if (EventStatus is EventStatus.Active)
            return Result.Success();

        if (EventStatus is EventStatus.Draft)
        {
            var readyResult = Ready();
            if (readyResult.IsFailure)
                return readyResult;
        }

        EventStatus = EventStatus.Active;
        return Result.Success();
    }

    public Result<None> MakePublic()
    {
        if (EventStatus is EventStatus.Cancelled)
            return Error.EventStatusIsCanceled;

        IsPublic = true;
        return Result.Success();
    }

    public Result<None> MakePrivate()
    {
        if (EventStatus is EventStatus.Active)
            return Error.EventStatusIsActive;

        if (EventStatus is EventStatus.Cancelled)
            return Error.EventStatusIsCanceled;

        IsPublic = false;
        return Result.Success();
    }

    public Result<None> SetMaxGuests(int max)
    {
        var errors = new HashSet<Error>();

        if (EventStatus is EventStatus.Cancelled)
            errors.Add(Error.EventStatusIsCanceled);

        if (max < 5)
            errors.Add(Error.TooFewGuests(5));

        if (max > 50)
            errors.Add(Error.TooManyGuests(50));

        if (EventStatus is EventStatus.Active && max < MaxGuests)
            errors.Add(Error.EventStatusIsActiveAndMaxGuestsReduced);

        if (errors.Count != 0)
            return Result.Failure<None>(errors);

        MaxGuests = max;
        return Result.Success();
    }

    public Result<None> RemoveParticipant(Email guestEmail)
    {
        // F1: Event already started
        if (EventStartDateTime <= DateTime.UtcNow)
            return Error.EventHasStarted;

        // S2: If not participating → do nothing
        if (!_participants.Contains(guestEmail))
            return Result.Success();

        // S1: Remove participant
        _participants.Remove(guestEmail);
        return Result.Success();
    }

    public Result<None> InviteGuest(Email guestEmail)
    {
        if (EventStatus != EventStatus.Ready && EventStatus != EventStatus.Active)
            return Error.EventNotReadyOrActive;

        if (_participants.Count >= MaxGuests)
            return Error.EventIsFull;

        if (_invitations.Contains(guestEmail))
            return Error.GuestAlreadyInvited;

        if (_participants.Contains(guestEmail))
            return Error.GuestAlreadyParticipating;

        _invitations.Add(guestEmail);
        return Result.Success();
    }

    public Result<None> AcceptInvitation(Email guestEmail)
    {
        if (EventStatus != EventStatus.Active)
        {
            if (EventStatus == EventStatus.Cancelled)
                return Error.EventCancelled;
            return Error.EventNotActive;
        }

        if (!_invitations.Contains(guestEmail))
            return Error.InvitationNotFound;

        if (_participants.Count >= MaxGuests)
            return Error.EventIsFull;

        _invitations.Remove(guestEmail);
        _participants.Add(guestEmail);

        return Result.Success();
    }

    public bool HasPendingInvitation(Email email) => _invitations.Contains(email);

    public Result<None> DeclineInvitation(Email guestEmail)
    {
        if (EventStatus == EventStatus.Cancelled)
            return Error.EventCancelled;

        if (!_invitations.Contains(guestEmail) && !_participants.Contains(guestEmail))
            return Error.InvitationNotFound;

        _invitations.Remove(guestEmail);
        _participants.Remove(guestEmail);
        _declinedInvitations.Add(guestEmail);

        return Result.Success();
    }

    public bool IsInvitationDeclined(Email email) => _declinedInvitations.Contains(email);
}