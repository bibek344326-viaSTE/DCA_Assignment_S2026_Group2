using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Handlers;

public class GetSingleEventQueryHandler(QueryDbContext context)
    : IQueryHandler<GetSingleEventQuery, SingleEventAnswer?>
{
    public async Task<SingleEventAnswer?> HandleAsync(GetSingleEventQuery query)
    {
        var eventData = await context.Events
            .AsNoTracking()
            .Include(e => e.Location)
            .FirstOrDefaultAsync(e => e.Id == query.EventId);

        if (eventData is null)
            return null;

        var guestOffset = Math.Max(0, query.GuestOffset);
        var guestPageSize = Math.Max(1, query.GuestPageSize);
        var attendeeIds = EventAttendeeQueries.AttendeeIds(context, query.EventId);
        var totalGuests = await attendeeIds.CountAsync();

        var guests = await context.Guests
            .AsNoTracking()
            .Where(g => attendeeIds.Contains(g.Id))
            .OrderBy(g => g.FirstName)
            .ThenBy(g => g.LastName)
            .Skip(guestOffset)
            .Take(guestPageSize)
            .Select(g => new EventGuestSummary(g.Id, g.FirstName, g.LastName, g.Url))
            .ToListAsync();

        return new SingleEventAnswer(
            eventData.Id,
            eventData.Title,
            eventData.Description,
            eventData.Location?.Name ?? string.Empty,
            eventData.Start,
            eventData.End,
            eventData.Visibility,
            totalGuests,
            eventData.MaxGuests,
            guestOffset,
            guestPageSize,
            totalGuests,
            guests);
    }
}
