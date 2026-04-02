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

    public EventFactory WithMaxNumberOfGuestsInvalid(int maxNumberOfGuests)
    {
        var property = typeof(EventRoot).GetProperty("maxGuests", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        property.SetValue(_event, maxNumberOfGuests);
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
    
    public EventFactory WithValidTitle()
    {
        _event.UpdateTitle("VIA Friday Bar");
        return this;
    }

    public EventFactory WithValidDescription()
    {
        _event.UpdateDescription("A nice event for students and guests.");
        return this;
    }

    public EventFactory WithTitle(string title)
    {
        _event.UpdateTitle(title);
        return this;
    }

    public EventFactory WithDescription(string description)
    {
        _event.UpdateDescription(description);
        return this;
    }
    
    public EventFactory WithDateTime(DateTime start, DateTime end)
    {
        _event.UpdateDateTime(start, end);
        return this;
    }

    public EventFactory WithValidTimeInFuture()
    {
        var start = DateTime.Now.AddDays(2).Date.AddHours(10);
        var end = start.AddHours(2);

        _event.UpdateDateTime(start, end);
        return this;
    }

    public EventFactory WithValidTimeInPast()
    {
        var start = DateTime.Now.AddDays(-2).Date.AddHours(10);
        var end = start.AddHours(2);

        _event.UpdateDateTime(start, end);
        return this;
    }   
    public EventFactory WithPublicVisibility()
    {
        _event.MakePublic();
        return this;
    }

    public EventFactory WithPrivateVisibility()
    {
        _event.MakePrivate();
        return this;
    }
}