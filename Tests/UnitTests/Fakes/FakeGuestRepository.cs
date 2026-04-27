using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

namespace UnitTests.Fakes;

public class FakeGuestRepository : FakeRepository<Guest, Email>, IGuestRepository
{
    public FakeGuestRepository() : base(g => g.Id)
    {
    }
}
