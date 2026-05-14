namespace ViaEventAssociation.Infrastructure.EfcQueries.Models;

public class InvitationReadModel
{
    public Guid EventId { get; set; }
    public Guid GuestId { get; set; }
    public string Status { get; set; } = string.Empty;

    public EventReadModel? Event { get; set; }
    public GuestReadModel? Guest { get; set; }
}
