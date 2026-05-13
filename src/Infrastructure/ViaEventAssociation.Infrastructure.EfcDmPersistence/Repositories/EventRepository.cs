using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;

public class EventRepository(DmContext context)
    : EfcRepository<EventRoot, EventId>(context), IEventRepository;
