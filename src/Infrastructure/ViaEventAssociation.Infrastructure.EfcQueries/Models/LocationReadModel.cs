namespace ViaEventAssociation.Infrastructure.EfcQueries.Models;

public class LocationReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public string AvailabilityStart { get; set; } = string.Empty;
    public string AvailabilityEnd { get; set; } = string.Empty;

    public ICollection<EventReadModel> Events { get; set; } = [];
}
