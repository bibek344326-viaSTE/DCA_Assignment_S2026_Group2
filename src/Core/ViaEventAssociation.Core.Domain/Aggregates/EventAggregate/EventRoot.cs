using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;

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

    public void UpdateTitle(string title)
    {
        if (eventStatus == EventStatus.Active)
            throw new InvalidOperationException("Active events cannot be modified");

        if (eventStatus == EventStatus.Cancelled)
            throw new InvalidOperationException("Cancelled events cannot be modified");

        eventTitle = title;

        if (eventStatus == EventStatus.Ready)
        {
            eventStatus = EventStatus.Draft;
        }
    }
    
    public void UpdateDescription(string description)
    {
        if (eventStatus == EventStatus.Active)
            throw new InvalidOperationException("Active events cannot be modified");

        if (eventStatus == EventStatus.Cancelled)
            throw new InvalidOperationException("Cancelled events cannot be modified");

        eventDescription = description;

        if (eventStatus == EventStatus.Ready)
        {
            eventStatus = EventStatus.Draft;
        }
    }
}