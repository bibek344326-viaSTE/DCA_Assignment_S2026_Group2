using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.UnitOfWork;

namespace IntegrationTests.Persistence.RepositoryTests;

public class GuestRepositoryTests
{
    [Fact]
    public async Task GuestRepository_SavesAndLoadsGuest()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new GuestRepository(fixture.Context);
        var unitOfWork = new EfcUnitOfWork(fixture.Context);
        var guest = Guest.Create("bibe@via.dk", "Bibek", "Chaudhary", "https://example.com/bibek.jpg").Payload!;

        await repository.AddAsync(guest);
        await unitOfWork.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        var result = await repository.GetByIdAsync(guest.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal(guest.Id, result.Payload!.Id);
        Assert.Equal("Bibek", result.Payload.FirstName);
        Assert.Equal("Chaudhary", result.Payload.LastName);
        Assert.Equal(new Uri("https://example.com/bibek.jpg"), result.Payload.ProfilePictureUrl);
    }
}
