using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventRoot : AggregateRoot<EventId> 
{
    internal EventTitle eventTitle { get; private set; }
    internal EventDescription eventDescription { get; private set; }
    internal DateTime eventStartDateTime { get; private set; }
    internal DateTime eventEndDateTime { get; private set; }
    internal EventVisibility Visibility { get; private set; }
    internal int maxGuests { get; private set; }
    internal EventStatus eventStatus { get; private set; }
    internal LocationId? locationId { get; private set; }

    private EventRoot(EventId id) : base(id)
    {
        eventTitle = EventTitle.Create("Working Title");
        eventDescription = EventDescription.Create(string.Empty);
        Visibility = EventVisibility.Private;
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
}