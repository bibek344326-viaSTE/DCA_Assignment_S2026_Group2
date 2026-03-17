using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

public class EventLocation : AggregateRoot<LocationId>
{
    internal string locationName { get; private set; }
    internal int maxNumberOfPeople { get; private set; }
    internal DateTime availabilityStart { get; private set; }
    internal DateTime availabilityEnd { get; private set; }

    private EventLocation(LocationId id) : base(id)
    {
        locationName = "Unnamed Location";
        maxNumberOfPeople = 5;
        availabilityStart = DateTime.MinValue;
        availabilityEnd = DateTime.MinValue;
    }

    public static EventLocation Create()
    {
        return new EventLocation(LocationId.Create());
    }
    
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Location name cannot be empty.");

        locationName = name;
    }
    
    public void SetMaxPeople(int max)
    {
        if (max < 1)
            throw new ArgumentException("Maximum number of people must be at least 1.");

        maxNumberOfPeople = max;
    }
    
    public void SetAvailability(DateTime start, DateTime end)
    {
        if (start >= end)
            throw new ArgumentException("Availability start must be before end.");

        availabilityStart = start;
        availabilityEnd = end;
    }
}