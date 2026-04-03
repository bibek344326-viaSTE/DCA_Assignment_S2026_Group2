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
}