using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventRoot : AggregateRoot<EventId> 
{
    internal string eventTitle { get; private set; }
    internal string eventDescription { get; private set; }
    internal DateTime eventStartDateTime { get; private set; }
    internal DateTime eventEndDateTime { get; private set; }
    internal bool isPublic { get; private set; }
    internal int maxGuests { get; private set; }
    internal EventStatus eventStatus { get; private set; }

    private EventRoot(EventId id) : base(id)
    {
        eventTitle = "Working Title";
        eventDescription = string.Empty;
        isPublic = false;
        maxGuests = 5;
        eventStatus = EventStatus.Draft;
    }
    
    public static EventRoot Create()
    {
        return new EventRoot(EventId.Create());
    }

    public void SetEventStatus(EventStatus status)
    {
        eventStatus = status;
    }

    public void SetMaxGuests(int max)
    {
        maxGuests = max;
    }

    public Result<None> UpdateTitle(string title)
    {
        if (eventStatus == EventStatus.Active)
            return Error.EventStatusIsActive;

        if (eventStatus == EventStatus.Cancelled)
            return Error.EventStatusIsCanceled;

        if (title is null)
            return Error.NullString;

        if (title.Length == 0)
            return Error.BlankString;

        if (title.Length < 3)
            return Error.TooShortTitle(3);

        if (title.Length > 75)
            return Error.TooLongTitle(75);

        eventTitle = title;

        if (eventStatus == EventStatus.Ready)
        {
            eventStatus = EventStatus.Draft;
        }

        return Result.Success();
    }
    
    public Result<None> UpdateDescription(string description)
    {
        if (eventStatus == EventStatus.Active)
            return Error.EventStatusIsActive;

        if (eventStatus == EventStatus.Cancelled)
            return Error.EventStatusIsCanceled;

        if (description is null || description.Length == 0)
        {
            eventDescription = string.Empty;

            if (eventStatus == EventStatus.Ready)
            {
                eventStatus = EventStatus.Draft;
            }

            return Result.Success();
        }

        if (description.Length > 250)
            return Error.TooLongDescription(250);

        eventDescription = description;

        if (eventStatus == EventStatus.Ready)
        {
            eventStatus = EventStatus.Draft;
        }

        return Result.Success();
    }
}