namespace ViaEventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<TId>
{
    public TId Id { get; }
    
    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;
        
        var other = (Entity<TId>)obj;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

