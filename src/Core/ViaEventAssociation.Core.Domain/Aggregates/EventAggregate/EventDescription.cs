using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventDescription : ValueObject
{
    public string Value { get; }

    private EventDescription(string value)
    {
        Value = value;
    }

    public static EventDescription Create(string description) => new(description);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

