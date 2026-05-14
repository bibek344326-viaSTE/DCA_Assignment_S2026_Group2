namespace ViaEventAssociation.Infrastructure.EfcQueries.Models;

public class EventReadModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int MaxGuests { get; set; }
    public Guid LocationId { get; set; }

    public LocationReadModel? Location { get; set; }
    public ICollection<ParticipationReadModel> Participations { get; set; } = [];
    public ICollection<InvitationReadModel> Invitations { get; set; } = [];
    public ICollection<JoinRequestReadModel> JoinRequests { get; set; } = [];
}
