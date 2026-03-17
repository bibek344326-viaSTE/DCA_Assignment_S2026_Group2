using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

namespace UnitTests.Features.Event;

public class EventFactory
{
    private EventRoot? _event;
    
    private EventFactory()
    {
    }
    
    public static EventFactory Init()
    {   
        var factory = new EventFactory();
        factory._event = EventRoot.Create();
        return factory;
    }
    
    public EventFactory WithStatus(EventStatus status) 
    {
        _event.SetEventStatus(status);
        return this;
    }

    public EventFactory WithMaxNumberOfGuests(int maxNumberOfGuests) 
    {
        _event.SetMaxGuests(maxNumberOfGuests);
        return this;
    }
    
    public EventRoot Build()
    {
        return _event;
    }
    
    public EventFactory WithLocation(LocationId locationId)
    {
        _event.SetLocation(locationId);
        return this;
    }
}