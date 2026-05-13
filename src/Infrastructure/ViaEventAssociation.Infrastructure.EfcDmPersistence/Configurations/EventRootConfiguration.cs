using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Configurations;

internal class EventRootConfiguration : IEntityTypeConfiguration<EventRoot>
{
    public void Configure(EntityTypeBuilder<EventRoot> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => EventId.FromGuid(value))
            .ValueGeneratedNever();

        builder.Property(e => e.EventTitle)
            .HasMaxLength(75)
            .IsRequired();

        builder.Property(e => e.EventDescription)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(e => e.EventStartDateTime);
        builder.Property(e => e.EventEndDateTime);
        builder.Property(e => e.IsPublic);
        builder.Property(e => e.MaxGuests);
        builder.Property(e => e.EventStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.LocationId)
            .HasConversion(id => ReferenceEquals(id, null) ? (Guid?)null : id.Value,
                value => value.HasValue ? LocationId.FromGuid(value.Value) : null);

        builder.Property(e => e.LocationMaxCapacity);

        ConfigureEmailCollection(builder, "_participants", "EventParticipants");
        ConfigureEmailCollection(builder, "_invitations", "EventInvitations");
        ConfigureEmailCollection(builder, "_declinedInvitations", "EventDeclinedInvitations");
    }

    private static void ConfigureEmailCollection(
        EntityTypeBuilder<EventRoot> builder,
        string fieldName,
        string tableName)
    {
        builder.OwnsMany<Email>(fieldName, owned =>
        {
            owned.ToTable(tableName);
            owned.WithOwner().HasForeignKey("EventId");
            owned.Property<int>("Id").ValueGeneratedOnAdd();
            owned.HasKey("Id");
            owned.Property(email => email.Value)
                .HasColumnName("Email")
                .HasMaxLength(254)
                .IsRequired();
        });

        builder.Navigation(fieldName).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
