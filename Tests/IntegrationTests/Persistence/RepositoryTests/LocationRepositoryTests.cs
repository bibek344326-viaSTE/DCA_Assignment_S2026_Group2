using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.UnitOfWork;

namespace IntegrationTests.Persistence.RepositoryTests;

public class LocationRepositoryTests
{
    [Fact]
    public async Task LocationRepository_SavesAndLoadsLocation()
    {
        using var fixture = new SqliteTestFixture();
        var repository = new LocationRepository(fixture.Context);
        var unitOfWork = new EfcUnitOfWork(fixture.Context);
        var location = EventLocation.Create();
        location.UpdateName("Auditorium A");
        location.SetMaxPeople(50);
        location.SetAvailability(new DateTime(2027, 5, 20, 8, 0, 0), new DateTime(2027, 5, 20, 18, 0, 0));

        await repository.AddAsync(location);
        await unitOfWork.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        var result = await repository.GetByIdAsync(location.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal(location.Id, result.Payload!.Id);
        Assert.Equal("Auditorium A", result.Payload.locationName);
        Assert.Equal(50, result.Payload.maxNumberOfPeople);
        Assert.Equal(new DateTime(2027, 5, 20, 8, 0, 0), result.Payload.availabilityStart);
        Assert.Equal(new DateTime(2027, 5, 20, 18, 0, 0), result.Payload.availabilityEnd);
    }
}
