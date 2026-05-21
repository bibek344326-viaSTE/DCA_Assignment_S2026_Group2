namespace ViaEventAssociation.Core.QueryContracts.Queries;

public sealed record GetPersonalProfileQuery(Guid GuestId) : IQuery<PersonalProfileAnswer?>;

public sealed record PersonalProfileAnswer(
    Guid GuestId,
    string FirstName,
    string LastName,
    string Email,
    string ProfilePictureUrl,
    int UpcomingEventCount,
    int PendingInvitationCount,
    IReadOnlyList<ProfileUpcomingEvent> UpcomingEvents,
    IReadOnlyList<ProfilePastEvent> PastEvents);

public sealed record ProfileUpcomingEvent(
    Guid EventId,
    string Title,
    int AttendeeCount,
    DateTime Date,
    DateTime StartTime);

public sealed record ProfilePastEvent(Guid EventId, string Title);
