using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Handlers;

public class GetEventEditingOverviewQueryHandler(QueryDbContext context)
    : IQueryHandler<GetEventEditingOverviewQuery, EventEditingOverviewAnswer>
{
    public async Task<EventEditingOverviewAnswer> HandleAsync(GetEventEditingOverviewQuery query)
    {
        var drafts = await GetEventsByStatusAsync("draft");
        var readied = await GetEventsByStatusAsync("ready");
        var cancelled = await GetEventsByStatusAsync("cancelled");

        return new EventEditingOverviewAnswer(drafts, readied, cancelled);
    }

    private async Task<IReadOnlyList<EventEditingListItem>> GetEventsByStatusAsync(string status)
        => await context.Events
            .AsNoTracking()
            .Where(e => e.Status == status)
            .OrderBy(e => e.Title)
            .Select(e => new EventEditingListItem(e.Id, e.Title))
            .ToListAsync();
}
