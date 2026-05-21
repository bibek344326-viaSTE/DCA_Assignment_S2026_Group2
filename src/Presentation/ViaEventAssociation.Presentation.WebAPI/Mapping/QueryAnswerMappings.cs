using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

namespace ViaEventAssociation.Presentation.WebAPI.Mapping;

public sealed class QueryAnswerMappings :
    IMapping<PersonalProfileAnswer, PersonalProfileResponse>,
    IMapping<SingleEventAnswer, SingleEventResponse>,
    IMapping<EventEditingOverviewAnswer, EventEditingOverviewResponse>,
    IMapping<EventGuestStatusOverviewAnswer, EventGuestStatusOverviewResponse>
{
    public PersonalProfileResponse Map(PersonalProfileAnswer source)
        => new(
            source.GuestId,
            source.FirstName,
            source.LastName,
            source.Email,
            source.ProfilePictureUrl,
            source.UpcomingEventCount,
            source.PendingInvitationCount,
            source.UpcomingEvents.Select(e => new ProfileUpcomingEventResponse(e.EventId, e.Title, e.AttendeeCount, e.Date, e.StartTime)).ToList(),
            source.PastEvents.Select(e => new ProfilePastEventResponse(e.EventId, e.Title)).ToList());

    public SingleEventResponse Map(SingleEventAnswer source)
        => new(
            source.EventId,
            source.Title,
            source.Description,
            source.LocationName,
            source.Start,
            source.End,
            source.Visibility,
            source.AttendeeCount,
            source.MaxGuests,
            source.GuestOffset,
            source.GuestPageSize,
            source.TotalGuests,
            source.Guests.Select(g => new EventGuestResponse(g.GuestId, g.FirstName, g.LastName, g.ProfilePictureUrl)).ToList());

    public EventEditingOverviewResponse Map(EventEditingOverviewAnswer source)
        => new(
            source.Drafts.Select(Map).ToList(),
            source.Readied.Select(Map).ToList(),
            source.Cancelled.Select(Map).ToList());

    public EventGuestStatusOverviewResponse Map(EventGuestStatusOverviewAnswer source)
        => new(
            source.EventId,
            source.Title,
            source.Participants.Select(Map).ToList(),
            source.PendingInvitations.Select(Map).ToList(),
            source.DeclinedInvitations.Select(Map).ToList(),
            source.PendingJoinRequests.Select(Map).ToList(),
            source.AcceptedJoinRequests.Select(Map).ToList(),
            source.DeclinedJoinRequests.Select(Map).ToList());

    private static EventEditingListItemResponse Map(EventEditingListItem source)
        => new(source.EventId, source.Title);

    private static EventGuestStatusItemResponse Map(EventGuestStatusItem source)
        => new(source.GuestId, source.FirstName, source.LastName, source.Email, source.Reason);
}
