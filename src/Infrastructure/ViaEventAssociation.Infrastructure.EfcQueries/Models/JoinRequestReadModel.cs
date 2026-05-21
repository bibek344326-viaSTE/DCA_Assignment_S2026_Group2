namespace ViaEventAssociation.Infrastructure.EfcQueries.Models;

public class JoinRequestReadModel
{
    public Guid EventId { get; set; }
    public Guid GuestId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public EventReadModel? Event { get; set; }
    public GuestReadModel? Guest { get; set; }
}
