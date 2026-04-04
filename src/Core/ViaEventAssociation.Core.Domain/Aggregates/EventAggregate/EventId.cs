using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

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

  // Result-based factories for command/input scenarios
  public static Result<EventId> Create(Guid value) => Result.Success(new EventId(value));

  public static Result<EventId> Create(string value)
      => Guid.TryParse(value, out var guid)
          ? Result.Success(new EventId(guid))
          : Result.Failure<EventId>(Error.BlankString);

  protected override IEnumerable<object?> GetEqualityComponents()
  {
    yield return Value;
  }
}
