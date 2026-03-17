using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

namespace UnitTests.Features.Location;

public class LocationFactory
{
    private readonly EventLocation _location;

    private LocationFactory()
    {
        _location = EventLocation.Create();
    }

    public static LocationFactory Init()
    {
        return new LocationFactory();
    }

    public LocationFactory WithMaxNumberOfPeople(int max)
    {
        _location.SetMaxPeople(max);
        return this;
    }

    public LocationFactory WithName(string name)
    {
        _location.UpdateName(name);
        return this;
    }

    public LocationFactory WithAvailability(DateTime start, DateTime end)
    {
        _location.SetAvailability(start, end);
        return this;
    }

    public EventLocation Build()
    {
        return _location;
    }
}