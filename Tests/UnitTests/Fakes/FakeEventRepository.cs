using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Fakes;

public class FakeEventRepository : FakeRepository<EventRoot, EventId>, IEventRepository
{
    public FakeEventRepository() : base(e => e.Id)
    {
    }
}

