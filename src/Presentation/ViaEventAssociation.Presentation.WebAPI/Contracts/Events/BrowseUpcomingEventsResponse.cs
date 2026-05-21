namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Events;

public sealed record BrowseUpcomingEventsResponse(
    int PageNumber,
    int PageSize,
    int TotalItems,
    int TotalPages,
    IReadOnlyList<UpcomingEventResponse> Events);

public sealed record UpcomingEventResponse(
    Guid Id,
    DateTime Start,
    string Title,
    string Description,
    int AttendeeCount,
    int MaxGuests,
    string Visibility);
