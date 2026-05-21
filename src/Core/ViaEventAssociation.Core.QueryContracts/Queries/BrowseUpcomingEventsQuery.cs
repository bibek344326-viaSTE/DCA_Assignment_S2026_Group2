namespace ViaEventAssociation.Core.QueryContracts.Queries;

public sealed record BrowseUpcomingEventsQuery(
    string? SearchText,
    int PageNumber,
    int PageSize) : IQuery<BrowseUpcomingEventsAnswer>;

public sealed record BrowseUpcomingEventsAnswer(
    int PageNumber,
    int PageSize,
    int TotalItems,
    int TotalPages,
    IReadOnlyList<UpcomingEventSummary> Events);

public sealed record UpcomingEventSummary(
    Guid Id,
    DateTime Start,
    string Title,
    string Description,
    int AttendeeCount,
    int MaxGuests,
    string Visibility);
