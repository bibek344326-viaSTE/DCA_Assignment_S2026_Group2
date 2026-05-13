using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;

public class GuestRepository(DmContext context)
    : EfcRepository<Guest, Email>(context), IGuestRepository;
