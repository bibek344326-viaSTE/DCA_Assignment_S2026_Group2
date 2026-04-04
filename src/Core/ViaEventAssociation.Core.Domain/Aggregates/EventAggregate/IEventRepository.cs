using ViaEventAssociation.Core.Domain.Common.Repository;

namespace ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

public interface IEventRepository : IRepository<EventRoot, EventId>
{
}
