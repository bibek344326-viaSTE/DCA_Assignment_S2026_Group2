using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.UnitOfWork;

namespace IntegrationTests.Persistence;

public class UseCasePersistenceTests
{
    [Fact]
    public async Task UC1_CreatorCreatesEmptyEvent_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = EventRoot.Create();

        await AddAndSaveAsync(fixture, repository, @event);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal(EventStatus.Draft, reloaded.Status);
        Assert.Equal("Working Title", reloaded.EventTitle);
        Assert.Equal(string.Empty, reloaded.EventDescription);
        Assert.False(reloaded.IsPublic);
        Assert.Equal(5, reloaded.MaxGuests);
    }

    [Fact]
    public async Task UC2_CreatorUpdatesEventTitle_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, EventRoot.Create());

        @event.UpdateTitle("Persistence Title");
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal("Persistence Title", reloaded.EventTitle);
    }

    [Fact]
    public async Task UC3_CreatorUpdatesEventDescription_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, EventRoot.Create());

        @event.UpdateDescription("Persistence description.");
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal("Persistence description.", reloaded.EventDescription);
    }

    [Fact]
    public async Task UC4_CreatorUpdatesEventDateAndTime_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, EventRoot.Create());
        var start = new DateTime(2027, 6, 1, 10, 0, 0);
        var end = new DateTime(2027, 6, 1, 12, 0, 0);

        @event.UpdateDateTime(start, end);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal(start, reloaded.EventStartDateTime);
        Assert.Equal(end, reloaded.EventEndDateTime);
    }

    [Fact]
    public async Task UC5_CreatorMakesEventPublic_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, EventRoot.Create());

        @event.MakePublic();
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.True(reloaded.IsPublic);
    }

    [Fact]
    public async Task UC6_CreatorMakesEventPrivate_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = EventRoot.Create();
        @event.MakePublic();
        @event = await PersistEventAsync(fixture, repository, @event);

        @event.MakePrivate();
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.False(reloaded.IsPublic);
    }

    [Fact]
    public async Task UC7_CreatorSetsMaxGuests_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, EventRoot.Create());

        @event.SetMaxGuests(25);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal(25, reloaded.MaxGuests);
    }

    [Fact]
    public async Task UC8_CreatorReadiesEvent_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, CreateValidDraftEvent());

        @event.Ready();
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal(EventStatus.Ready, reloaded.Status);
    }

    [Fact]
    public async Task UC9_CreatorActivatesEvent_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var @event = await PersistEventAsync(fixture, repository, CreateValidDraftEvent());

        @event.Activate();
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.Equal(EventStatus.Active, reloaded.Status);
    }

    [Fact]
    public async Task UC10_AnonymousRegistersGuestAccount_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new GuestRepository(fixture.Context);
        var guest = Guest.Create("bibe@via.dk", "Bibek", "Chaudhary", "https://example.com/bibek.jpg").Payload!;

        await repository.AddAsync(guest);
        await SaveAndClearAsync(fixture);

        var result = await repository.GetByIdAsync(guest.Id);
        Assert.True(result.IsSuccess);
        Assert.Equal("Bibek", result.Payload!.FirstName);
        Assert.Equal("Chaudhary", result.Payload.LastName);
    }

    [Fact]
    public async Task UC11_GuestParticipatesInPublicEvent_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var email = Email.Create("bibe@via.dk").Payload!;
        var @event = await PersistEventAsync(fixture, repository, CreateActivePublicEvent());

        @event.AddParticipant(email);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.True(reloaded.IsParticipant(email));
        Assert.Equal(1, reloaded.GetCurrentParticipantCount());
    }

    [Fact]
    public async Task UC12_GuestCancelsParticipation_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var email = Email.Create("bibe@via.dk").Payload!;
        var @event = CreateActivePublicEvent();
        @event.AddParticipant(email);
        @event = await PersistEventAsync(fixture, repository, @event);

        @event.RemoveParticipant(email);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.False(reloaded.IsParticipant(email));
        Assert.Equal(0, reloaded.GetCurrentParticipantCount());
    }

    [Fact]
    public async Task UC13_CreatorInvitesGuestToEvent_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var email = Email.Create("bibe@via.dk").Payload!;
        var @event = await PersistEventAsync(fixture, repository, CreateReadyEvent());

        @event.InviteGuest(email);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.True(reloaded.HasPendingInvitation(email));
    }

    [Fact]
    public async Task UC14_GuestAcceptsInvitation_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var email = Email.Create("bibe@via.dk").Payload!;
        var @event = CreateActivePublicEvent();
        @event.InviteGuest(email);
        @event = await PersistEventAsync(fixture, repository, @event);

        @event.AcceptInvitation(email);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.True(reloaded.IsParticipant(email));
        Assert.False(reloaded.HasPendingInvitation(email));
    }

    [Fact]
    public async Task UC15_GuestDeclinesInvitation_Persists()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var email = Email.Create("bibe@via.dk").Payload!;
        var @event = CreateReadyEvent();
        @event.InviteGuest(email);
        @event = await PersistEventAsync(fixture, repository, @event);

        @event.DeclineInvitation(email);
        await SaveAndClearAsync(fixture);

        var reloaded = await ReloadEventAsync(fixture, repository, @event.Id);
        Assert.True(reloaded.IsInvitationDeclined(email));
        Assert.False(reloaded.HasPendingInvitation(email));
    }

    private static async Task AddAndSaveAsync(SqliteTestFixture fixture, EventRepository repository, EventRoot @event)
    {
        await repository.AddAsync(@event);
        await SaveAndClearAsync(fixture);
    }

    private static async Task<EventRoot> PersistEventAsync(SqliteTestFixture fixture, EventRepository repository, EventRoot @event)
    {
        await repository.AddAsync(@event);
        await SaveAndClearAsync(fixture);
        return await ReloadEventAsync(fixture, repository, @event.Id);
    }

    private static async Task<EventRoot> ReloadEventAsync(SqliteTestFixture fixture, EventRepository repository, EventId eventId)
    {
        var result = await repository.GetByIdAsync(eventId);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        return result.Payload!;
    }

    private static async Task SaveAndClearAsync(SqliteTestFixture fixture)
    {
        var unitOfWork = new EfcUnitOfWork(fixture.Context);
        await unitOfWork.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();
    }

    private static EventRoot CreateValidDraftEvent()
    {
        var @event = EventRoot.Create();
        @event.UpdateTitle("Persistence Event");
        @event.UpdateDescription("A valid event persisted through EF Core.");
        @event.UpdateDateTime(new DateTime(2027, 6, 1, 10, 0, 0), new DateTime(2027, 6, 1, 12, 0, 0));
        return @event;
    }

    private static EventRoot CreateReadyEvent()
    {
        var @event = CreateValidDraftEvent();
        @event.Ready();
        return @event;
    }

    private static EventRoot CreateActivePublicEvent()
    {
        var @event = CreateValidDraftEvent();
        @event.MakePublic();
        @event.Activate();
        return @event;
    }
}
