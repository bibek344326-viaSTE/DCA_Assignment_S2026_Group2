using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;

namespace ViaEventAssociation.Presentation.WebAPI.Mapping;

public sealed class BrowseUpcomingEventsAnswerMapping : IMapping<BrowseUpcomingEventsAnswer, BrowseUpcomingEventsResponse>
{
    public BrowseUpcomingEventsResponse Map(BrowseUpcomingEventsAnswer source)
        => new(
            source.PageNumber,
            source.PageSize,
            source.TotalItems,
            source.TotalPages,
            source.Events
                .Select(@event => new UpcomingEventResponse(
                    @event.Id,
                    @event.Start,
                    @event.Title,
                    @event.Description,
                    @event.AttendeeCount,
                    @event.MaxGuests,
                    @event.Visibility))
                .ToList());
}
