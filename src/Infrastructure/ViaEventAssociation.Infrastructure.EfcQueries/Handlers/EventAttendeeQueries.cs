using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Handlers;

internal static class EventAttendeeQueries
{
    public static IQueryable<Guid> AttendeeIds(QueryDbContext context, Guid eventId)
    {
        var participantIds = context.Participations
            .Where(p => p.EventId == eventId)
            .Select(p => p.GuestId);

        var acceptedInvitationIds = context.Invitations
            .Where(i => i.EventId == eventId && i.Status == "Accepted")
            .Select(i => i.GuestId);

        var acceptedJoinRequestIds = context.JoinRequests
            .Where(j => j.EventId == eventId && j.Status == "Accepted")
            .Select(j => j.GuestId);

        return participantIds
            .Concat(acceptedInvitationIds)
            .Concat(acceptedJoinRequestIds)
            .Distinct();
    }

    public static Task<int> CountAttendeesAsync(QueryDbContext context, Guid eventId)
        => AttendeeIds(context, eventId).CountAsync();
}
