namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Events;

public sealed record BrowseUpcomingEventsRequest(string? SearchText, int PageNumber = 1, int PageSize = 10);
