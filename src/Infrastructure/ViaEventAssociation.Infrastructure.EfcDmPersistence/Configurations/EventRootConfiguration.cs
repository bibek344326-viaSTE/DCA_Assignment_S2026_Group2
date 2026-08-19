using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using System;

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
        ConfigureInvitationCollection(builder, "_invitations", "EventInvitations");
        ConfigureJoinRequestCollection(builder, "_joiningRequests", "EventJoiningRequests");
        ConfigureAttendanceCollection(builder, "_attendances", "EventAttendances");
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

    private static void ConfigureInvitationCollection(
        EntityTypeBuilder<EventRoot> builder,
        string fieldName,
        string tableName)
    {
        builder.OwnsMany<Invitation>(fieldName, owned =>
        {
            owned.ToTable(tableName);
            owned.WithOwner().HasForeignKey("EventId");
            owned.Property<int>("Id").ValueGeneratedOnAdd();
            owned.HasKey("Id");

            owned.OwnsOne(invitation => invitation.GuestEmail, email =>
            {
                email.Property(value => value.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(254)
                    .IsRequired();
            });

            owned.Property(invitation => invitation.SentDate);
            owned.Property(invitation => invitation.InvitationStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.Navigation(fieldName).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureJoinRequestCollection(
        EntityTypeBuilder<EventRoot> builder,
        string fieldName,
        string tableName)
    {
        builder.OwnsMany<EventJoiningRequest>(fieldName, owned =>
        {
            owned.ToTable(tableName);
            owned.WithOwner().HasForeignKey("EventId");
            owned.Property<int>("Id").ValueGeneratedOnAdd();
            owned.HasKey("Id");

            owned.OwnsOne(request => request.GuestEmail, email =>
            {
                email.Property(value => value.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(254)
                    .IsRequired();
            });

            owned.Property(request => request.DescriptionOfJoining)
                .HasMaxLength(250)
                .IsRequired();
            owned.Property(request => request.RequestDate);
            owned.Property(request => request.ApprovalStatus)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.Navigation(fieldName).UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureAttendanceCollection(
        EntityTypeBuilder<EventRoot> builder,
        string fieldName,
        string tableName)
    {
        builder.OwnsMany<EventAttendance>(fieldName, owned =>
        {
            owned.ToTable(tableName);
            owned.WithOwner().HasForeignKey("EventId");
            owned.Property<int>("Id").ValueGeneratedOnAdd();
            owned.HasKey("Id");

            owned.OwnsOne(attendance => attendance.GuestEmail, email =>
            {
                email.Property(value => value.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(254)
                    .IsRequired();
            });

            owned.Property(attendance => attendance.RegisteredDate);
            owned.Property(attendance => attendance.IsCancelled);
        });

        builder.Navigation(fieldName).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
