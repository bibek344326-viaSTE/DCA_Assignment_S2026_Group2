using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public class EventId: ValueObject
{
  public Guid Value { get; }

  private EventId(Guid value)
  {
    Value = value;
  }
  
  public static EventId Create() => new(Guid.NewGuid());
  
  public static EventId FromGuid(Guid guid) => new(guid);

  protected override IEnumerable<object?> GetEqualityComponents()
  {
    yield return Value;
  }
}