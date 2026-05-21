namespace ViaEventAssociation.Infrastructure.EfcQueries.Models;

public class GuestReadModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public ICollection<ParticipationReadModel> Participations { get; set; } = [];
    public ICollection<InvitationReadModel> Invitations { get; set; } = [];
    public ICollection<JoinRequestReadModel> JoinRequests { get; set; } = [];
}
