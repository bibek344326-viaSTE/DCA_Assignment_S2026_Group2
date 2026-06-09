using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class RequestId : ValueObject
{
    public Guid Value { get; }

    private RequestId(Guid value)
    {
        Value = value;
    }

    public static RequestId Create() => new(Guid.NewGuid());

    public static RequestId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
