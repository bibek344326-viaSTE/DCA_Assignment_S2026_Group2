namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

public sealed record PersonalProfileResponse(
    Guid GuestId,
    string FirstName,
    string LastName,
    string Email,
    string ProfilePictureUrl,
    int UpcomingEventCount,
    int PendingInvitationCount,
    IReadOnlyList<ProfileUpcomingEventResponse> UpcomingEvents,
    IReadOnlyList<ProfilePastEventResponse> PastEvents);

public sealed record ProfileUpcomingEventResponse(
    Guid EventId,
    string Title,
    int AttendeeCount,
    DateTime Date,
    DateTime StartTime);

public sealed record ProfilePastEventResponse(Guid EventId, string Title);
