using ViaEventAssociation.Core.Domain.Common.Repository;

namespace ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

public interface ILocationRepository : IRepository<EventLocation, LocationId>
{
}
