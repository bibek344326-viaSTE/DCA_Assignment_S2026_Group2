using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class InvitationId : ValueObject
{
    public Guid Value { get; }

    private InvitationId(Guid value)
    {
        Value = value;
    }

    public static InvitationId Create() => new(Guid.NewGuid());

    public static InvitationId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
