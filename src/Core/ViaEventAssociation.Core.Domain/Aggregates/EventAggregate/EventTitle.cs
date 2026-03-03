using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventTitle : ValueObject
{
    public string Value { get; }

    private EventTitle(string value)
    {
        Value = value;
    }

    public static EventTitle Create(string title) => new(title);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}

