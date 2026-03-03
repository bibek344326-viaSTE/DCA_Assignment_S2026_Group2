namespace ViaEventAssociation.Core.Domain.Common.Bases;

public  abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType()!= GetType())
            return false;
        
        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(0, (hash, obj) =>
                HashCode.Combine(hash, obj));
    }

    public static bool operator ==(ValueObject a, ValueObject b) {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;
        return a.Equals(b);
    }
    
    public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);
    
}