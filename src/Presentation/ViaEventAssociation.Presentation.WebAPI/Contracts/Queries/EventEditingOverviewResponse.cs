namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

public sealed record EventEditingOverviewResponse(
    IReadOnlyList<EventEditingListItemResponse> Drafts,
    IReadOnlyList<EventEditingListItemResponse> Readied,
    IReadOnlyList<EventEditingListItemResponse> Cancelled);

public sealed record EventEditingListItemResponse(Guid EventId, string Title);
