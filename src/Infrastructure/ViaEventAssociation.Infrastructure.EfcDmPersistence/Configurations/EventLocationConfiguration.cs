using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Configurations;

internal class EventLocationConfiguration : IEntityTypeConfiguration<EventLocation>
{
    public void Configure(EntityTypeBuilder<EventLocation> builder)
    {
        builder.ToTable("Locations");

        builder.HasKey(location => location.Id);

        builder.Property(location => location.Id)
            .HasConversion(id => id.Value, value => LocationId.FromGuid(value))
            .ValueGeneratedNever();

        builder.Property(location => location.locationName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(location => location.maxNumberOfPeople);
        builder.Property(location => location.availabilityStart);
        builder.Property(location => location.availabilityEnd);
    }
}
