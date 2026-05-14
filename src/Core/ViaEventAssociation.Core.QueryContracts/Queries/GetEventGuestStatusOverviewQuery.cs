namespace ViaEventAssociation.Core.QueryContracts.Queries;

public sealed record GetEventGuestStatusOverviewQuery(Guid EventId) : IQuery<EventGuestStatusOverviewAnswer?>;

public sealed record EventGuestStatusOverviewAnswer(
    Guid EventId,
    string Title,
    IReadOnlyList<EventGuestStatusItem> Participants,
    IReadOnlyList<EventGuestStatusItem> PendingInvitations,
    IReadOnlyList<EventGuestStatusItem> DeclinedInvitations,
    IReadOnlyList<EventGuestStatusItem> PendingJoinRequests,
    IReadOnlyList<EventGuestStatusItem> AcceptedJoinRequests,
    IReadOnlyList<EventGuestStatusItem> DeclinedJoinRequests);

public sealed record EventGuestStatusItem(
    Guid GuestId,
    string FirstName,
    string LastName,
    string Email,
    string? Reason);
