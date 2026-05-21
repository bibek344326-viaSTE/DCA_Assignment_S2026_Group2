using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.Models;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

public class QueryDbContext(DbContextOptions<QueryDbContext> options) : DbContext(options)
{
    public DbSet<EventReadModel> Events => Set<EventReadModel>();
    public DbSet<GuestReadModel> Guests => Set<GuestReadModel>();
    public DbSet<LocationReadModel> Locations => Set<LocationReadModel>();
    public DbSet<ParticipationReadModel> Participations => Set<ParticipationReadModel>();
    public DbSet<InvitationReadModel> Invitations => Set<InvitationReadModel>();
    public DbSet<JoinRequestReadModel> JoinRequests => Set<JoinRequestReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventReadModel>(builder =>
        {
            builder.ToTable("Events");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).HasMaxLength(75).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(250).IsRequired();
            builder.Property(e => e.Status).HasMaxLength(20).IsRequired();
            builder.Property(e => e.Visibility).HasMaxLength(20).IsRequired();
            builder.HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId);
        });

        modelBuilder.Entity<GuestReadModel>(builder =>
        {
            builder.ToTable("Guests");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.FirstName).HasMaxLength(25).IsRequired();
            builder.Property(g => g.LastName).HasMaxLength(25).IsRequired();
            builder.Property(g => g.Email).HasMaxLength(254).IsRequired();
            builder.Property(g => g.Url).HasMaxLength(2048).IsRequired();
        });

        modelBuilder.Entity<LocationReadModel>(builder =>
        {
            builder.ToTable("Locations");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<ParticipationReadModel>(builder =>
        {
            builder.ToTable("Participations");
            builder.HasKey(p => new { p.EventId, p.GuestId });
            builder.HasOne(p => p.Event)
                .WithMany(e => e.Participations)
                .HasForeignKey(p => p.EventId);
            builder.HasOne(p => p.Guest)
                .WithMany(g => g.Participations)
                .HasForeignKey(p => p.GuestId);
        });

        modelBuilder.Entity<InvitationReadModel>(builder =>
        {
            builder.ToTable("Invitations");
            builder.HasKey(i => new { i.EventId, i.GuestId });
            builder.Property(i => i.Status).HasMaxLength(20).IsRequired();
            builder.HasOne(i => i.Event)
                .WithMany(e => e.Invitations)
                .HasForeignKey(i => i.EventId);
            builder.HasOne(i => i.Guest)
                .WithMany(g => g.Invitations)
                .HasForeignKey(i => i.GuestId);
        });

        modelBuilder.Entity<JoinRequestReadModel>(builder =>
        {
            builder.ToTable("JoinRequests");
            builder.HasKey(j => new { j.EventId, j.GuestId });
            builder.Property(j => j.Reason).HasMaxLength(250).IsRequired();
            builder.Property(j => j.Status).HasMaxLength(20).IsRequired();
            builder.HasOne(j => j.Event)
                .WithMany(e => e.JoinRequests)
                .HasForeignKey(j => j.EventId);
            builder.HasOne(j => j.Guest)
                .WithMany(g => g.JoinRequests)
                .HasForeignKey(j => j.GuestId);
        });
    }
}
