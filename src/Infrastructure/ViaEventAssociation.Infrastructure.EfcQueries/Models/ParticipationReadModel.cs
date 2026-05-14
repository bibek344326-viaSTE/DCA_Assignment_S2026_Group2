namespace ViaEventAssociation.Infrastructure.EfcQueries.Models;

public class ParticipationReadModel
{
    public Guid EventId { get; set; }
    public Guid GuestId { get; set; }

    public EventReadModel? Event { get; set; }
    public GuestReadModel? Guest { get; set; }
}
