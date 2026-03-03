using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

public class LocationId: ValueObject
{
  public Guid Value { get; }

  private LocationId(Guid value)
  {
    Value = value;
  }
  
  public static LocationId Create() => new(Guid.NewGuid());
  
  public static LocationId FromGuid(Guid guid) => new(guid);

  protected override IEnumerable<object?> GetEqualityComponents()
  {
    yield return Value;
  }
}

