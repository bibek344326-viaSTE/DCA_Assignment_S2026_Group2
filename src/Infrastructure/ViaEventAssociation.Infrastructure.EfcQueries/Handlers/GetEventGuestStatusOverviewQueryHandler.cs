using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Handlers;

public class GetEventGuestStatusOverviewQueryHandler(QueryDbContext context)
    : IQueryHandler<GetEventGuestStatusOverviewQuery, EventGuestStatusOverviewAnswer?>
{
    public async Task<EventGuestStatusOverviewAnswer?> HandleAsync(GetEventGuestStatusOverviewQuery query)
    {
        var eventData = await context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.EventId);

        if (eventData is null)
            return null;

        var participants = await context.Participations
            .AsNoTracking()
            .Where(p => p.EventId == query.EventId)
            .OrderBy(p => p.Guest!.FirstName)
            .ThenBy(p => p.Guest!.LastName)
            .Select(p => new EventGuestStatusItem(
                p.GuestId,
                p.Guest!.FirstName,
                p.Guest.LastName,
                p.Guest.Email,
                null))
            .ToListAsync();

        var pendingInvitations = await GetInvitationsAsync(query.EventId, "Pending");
        var declinedInvitations = await GetInvitationsAsync(query.EventId, "Rejected");
        var pendingJoinRequests = await GetJoinRequestsAsync(query.EventId, "Pending");
        var acceptedJoinRequests = await GetJoinRequestsAsync(query.EventId, "Accepted");
        var declinedJoinRequests = await GetJoinRequestsAsync(query.EventId, "Rejected");

        return new EventGuestStatusOverviewAnswer(
            eventData.Id,
            eventData.Title,
            participants,
            pendingInvitations,
            declinedInvitations,
            pendingJoinRequests,
            acceptedJoinRequests,
            declinedJoinRequests);
    }

    private async Task<IReadOnlyList<EventGuestStatusItem>> GetInvitationsAsync(Guid eventId, string status)
        => await context.Invitations
            .AsNoTracking()
            .Where(i => i.EventId == eventId && i.Status == status)
            .OrderBy(i => i.Guest!.FirstName)
            .ThenBy(i => i.Guest!.LastName)
            .Select(i => new EventGuestStatusItem(
                i.GuestId,
                i.Guest!.FirstName,
                i.Guest.LastName,
                i.Guest.Email,
                null))
            .ToListAsync();

    private async Task<IReadOnlyList<EventGuestStatusItem>> GetJoinRequestsAsync(Guid eventId, string status)
        => await context.JoinRequests
            .AsNoTracking()
            .Where(j => j.EventId == eventId && j.Status == status)
            .OrderBy(j => j.Guest!.FirstName)
            .ThenBy(j => j.Guest!.LastName)
            .Select(j => new EventGuestStatusItem(
                j.GuestId,
                j.Guest!.FirstName,
                j.Guest.LastName,
                j.Guest.Email,
                j.Reason))
            .ToListAsync();
}
