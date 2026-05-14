using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Handlers;

public class GetPersonalProfileQueryHandler(QueryDbContext context, ISystemTime systemTime)
    : IQueryHandler<GetPersonalProfileQuery, PersonalProfileAnswer?>
{
    public async Task<PersonalProfileAnswer?> HandleAsync(GetPersonalProfileQuery query)
    {
        var guest = await context.Guests
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == query.GuestId);

        if (guest is null)
            return null;

        var participatingEventIds = context.Participations
            .Where(p => p.GuestId == query.GuestId)
            .Select(p => p.EventId);

        var acceptedInvitationEventIds = context.Invitations
            .Where(i => i.GuestId == query.GuestId && i.Status == "Accepted")
            .Select(i => i.EventId);

        var acceptedJoinRequestEventIds = context.JoinRequests
            .Where(j => j.GuestId == query.GuestId && j.Status == "Accepted")
            .Select(j => j.EventId);

        var attendedEventIds = participatingEventIds
            .Concat(acceptedInvitationEventIds)
            .Concat(acceptedJoinRequestEventIds)
            .Distinct();

        var upcomingBase = context.Events
            .AsNoTracking()
            .Where(e => attendedEventIds.Contains(e.Id) && e.Start > systemTime.Now);

        var upcomingEvents = await upcomingBase
            .OrderBy(e => e.Start)
            .Select(e => new ProfileUpcomingEvent(
                e.Id,
                e.Title,
                e.Participations.Select(p => p.GuestId)
                    .Concat(e.Invitations.Where(i => i.Status == "Accepted").Select(i => i.GuestId))
                    .Concat(e.JoinRequests.Where(j => j.Status == "Accepted").Select(j => j.GuestId))
                    .Distinct()
                    .Count(),
                e.Start.Date,
                e.Start))
            .ToListAsync();

        var pastEvents = await context.Events
            .AsNoTracking()
            .Where(e => attendedEventIds.Contains(e.Id) && e.Start <= systemTime.Now)
            .OrderByDescending(e => e.Start)
            .Take(5)
            .Select(e => new ProfilePastEvent(e.Id, e.Title))
            .ToListAsync();

        var pendingInvitationCount = await context.Invitations
            .AsNoTracking()
            .CountAsync(i => i.GuestId == query.GuestId && i.Status == "Pending");

        return new PersonalProfileAnswer(
            guest.Id,
            guest.FirstName,
            guest.LastName,
            guest.Email,
            guest.Url,
            upcomingEvents.Count,
            pendingInvitationCount,
            upcomingEvents,
            pastEvents);
    }
}
