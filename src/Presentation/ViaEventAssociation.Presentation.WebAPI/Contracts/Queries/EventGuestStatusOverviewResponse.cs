namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

public sealed record EventGuestStatusOverviewResponse(
    Guid EventId,
    string Title,
    IReadOnlyList<EventGuestStatusItemResponse> Participants,
    IReadOnlyList<EventGuestStatusItemResponse> PendingInvitations,
    IReadOnlyList<EventGuestStatusItemResponse> DeclinedInvitations,
    IReadOnlyList<EventGuestStatusItemResponse> PendingJoinRequests,
    IReadOnlyList<EventGuestStatusItemResponse> AcceptedJoinRequests,
    IReadOnlyList<EventGuestStatusItemResponse> DeclinedJoinRequests);

public sealed record EventGuestStatusItemResponse(
    Guid GuestId,
    string FirstName,
    string LastName,
    string Email,
    string? Reason);
