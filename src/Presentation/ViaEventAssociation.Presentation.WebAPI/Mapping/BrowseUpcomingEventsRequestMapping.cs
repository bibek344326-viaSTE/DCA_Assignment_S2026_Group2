using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;

namespace ViaEventAssociation.Presentation.WebAPI.Mapping;

public sealed class BrowseUpcomingEventsRequestMapping : IMapping<BrowseUpcomingEventsRequest, BrowseUpcomingEventsQuery>
{
    public BrowseUpcomingEventsQuery Map(BrowseUpcomingEventsRequest source)
        => new(source.SearchText, Math.Max(1, source.PageNumber), Math.Max(1, source.PageSize));
}
