namespace ViaEventAssociation.Core.QueryContracts.Queries;

public sealed record GetEventEditingOverviewQuery() : IQuery<EventEditingOverviewAnswer>;

public sealed record EventEditingOverviewAnswer(
    IReadOnlyList<EventEditingListItem> Drafts,
    IReadOnlyList<EventEditingListItem> Readied,
    IReadOnlyList<EventEditingListItem> Cancelled);

public sealed record EventEditingListItem(Guid EventId, string Title);
