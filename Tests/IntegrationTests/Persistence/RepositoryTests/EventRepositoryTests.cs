using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.UnitOfWork;

namespace IntegrationTests.Persistence.RepositoryTests;

public class EventRepositoryTests
{
    [Fact]
    public async Task EventRepository_SavesAndLoadsDraftEvent()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var unitOfWork = new EfcUnitOfWork(fixture.Context);
        var eventId = EventId.Create();
        var @event = EventRoot.Create(eventId);

        await repository.AddAsync(@event);
        await unitOfWork.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        var result = await repository.GetByIdAsync(eventId);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(EventStatus.Draft, result.Payload!.Status);
    }

    [Fact]
    public async Task EventRepository_SavesAndLoadsParticipationAndInvitationState()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new EventRepository(fixture.Context);
        var unitOfWork = new EfcUnitOfWork(fixture.Context);
        var participant = Email.Create("bibe@via.dk").Payload!;
        var invited = Email.Create("343876@via.dk").Payload!;
        var declined = Email.Create("sneh@via.dk").Payload!;
        var @event = CreateActivePublicEvent();

        @event.AddParticipant(participant);
        @event.InviteGuest(invited);
        @event.InviteGuest(declined);
        @event.DeclineInvitation(declined);

        await repository.AddAsync(@event);
        await unitOfWork.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        var result = await repository.GetByIdAsync(@event.Id);

        Assert.True(result.IsSuccess);
        Assert.True(result.Payload!.IsParticipant(participant));
        Assert.True(result.Payload.HasPendingInvitation(invited));
        Assert.True(result.Payload.IsInvitationDeclined(declined));
        Assert.Equal(1, result.Payload.GetCurrentParticipantCount());
    }

    private static EventRoot CreateActivePublicEvent()
    {
        var @event = EventRoot.Create();
        @event.UpdateTitle("Persistence Event");
        @event.UpdateDescription("A valid event persisted through EF Core.");
        @event.UpdateDateTime(new DateTime(2027, 5, 20, 10, 0, 0), new DateTime(2027, 5, 20, 12, 0, 0));
        @event.MakePublic();
        @event.Activate();
        return @event;
    }
}
