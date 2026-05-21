using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Handlers;

public class BrowseUpcomingEventsQueryHandler(
    QueryDbContext context,
    ISystemTime systemTime,
    IMemoryCache cache)
    : IQueryHandler<BrowseUpcomingEventsQuery, BrowseUpcomingEventsAnswer>
{
    public async Task<BrowseUpcomingEventsAnswer> HandleAsync(BrowseUpcomingEventsQuery query)
    {
        var pageNumber = Math.Max(1, query.PageNumber);
        var pageSize = Math.Max(1, query.PageSize);
        var searchText = query.SearchText?.Trim();
        var cacheKey = $"upcoming-events:{systemTime.Now:yyyy-MM-dd}:{searchText}:{pageNumber}:{pageSize}";

        if (cache.TryGetValue(cacheKey, out BrowseUpcomingEventsAnswer? cached) && cached is not null)
            return cached;

        var eventsQuery = context.Events
            .AsNoTracking()
            .Where(e => e.Start > systemTime.Now);

        if (!string.IsNullOrWhiteSpace(searchText))
            eventsQuery = eventsQuery.Where(e => e.Title.Contains(searchText));

        var totalItems = await eventsQuery.CountAsync();
        var totalPages = totalItems == 0
            ? 0
            : (int)Math.Ceiling(totalItems / (double)pageSize);

        var events = await eventsQuery
            .OrderBy(e => e.Start)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new UpcomingEventSummary(
                e.Id,
                e.Start,
                e.Title,
                e.Description,
                e.Participations.Select(p => p.GuestId)
                    .Concat(e.Invitations
                        .Where(i => i.Status == "Accepted")
                        .Select(i => i.GuestId))
                    .Concat(e.JoinRequests
                        .Where(j => j.Status == "Accepted")
                        .Select(j => j.GuestId))
                    .Distinct()
                    .Count(),
                e.MaxGuests,
                e.Visibility))
            .ToListAsync();

        var answer = new BrowseUpcomingEventsAnswer(
            pageNumber,
            pageSize,
            totalItems,
            totalPages,
            events);

        cache.Set(cacheKey, answer, TimeSpan.FromHours(1));
        return answer;
    }
}
