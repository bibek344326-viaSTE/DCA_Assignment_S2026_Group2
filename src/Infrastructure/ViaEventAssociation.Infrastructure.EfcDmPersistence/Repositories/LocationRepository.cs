using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;

public class LocationRepository(DmContext context)
    : EfcRepository<EventLocation, LocationId>(context), ILocationRepository;
