using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

public class DmContext(DbContextOptions<DmContext> options) : DbContext(options)
{
    public DbSet<EventRoot> Events => Set<EventRoot>();

    public DbSet<Guest> Guests => Set<Guest>();

    public DbSet<EventLocation> Locations => Set<EventLocation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DmContext).Assembly);
    }
}
