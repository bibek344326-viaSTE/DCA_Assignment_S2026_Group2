using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Configurations;

internal class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasConversion(id => id.Value, value => Email.Create(value).Payload!)
            .HasMaxLength(254)
            .ValueGeneratedNever();

        builder.Property(g => g.email)
            .HasConversion(email => email.Value, value => Email.Create(value).Payload!)
            .HasColumnName("Email")
            .HasMaxLength(254)
            .IsRequired();

        builder.Property(g => g.FirstName)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(g => g.LastName)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(g => g.ProfilePictureUrl)
            .HasConversion(uri => uri.ToString(), value => new Uri(value))
            .HasMaxLength(2048)
            .IsRequired();
    }
}
