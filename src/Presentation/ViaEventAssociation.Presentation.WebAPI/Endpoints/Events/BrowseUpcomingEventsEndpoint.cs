using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events/upcoming")]
public sealed class BrowseUpcomingEventsEndpoint(
    IQueryDispatcher queryDispatcher,
    IObjectMapper mapper)
    : ApiEndpoint<BrowseUpcomingEventsRequest, BrowseUpcomingEventsResponse>
{
    [HttpGet]
    public override async Task<ActionResult<BrowseUpcomingEventsResponse>> HandleAsync([FromQuery] BrowseUpcomingEventsRequest request)
    {
        try
        {
            var query = mapper.Map<BrowseUpcomingEventsRequest, BrowseUpcomingEventsQuery>(request);
            var answer = await queryDispatcher.DispatchAsync(query);
            return Ok(mapper.Map<BrowseUpcomingEventsAnswer, BrowseUpcomingEventsResponse>(answer));
        }
        catch (Exception exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
        }
    }
}
