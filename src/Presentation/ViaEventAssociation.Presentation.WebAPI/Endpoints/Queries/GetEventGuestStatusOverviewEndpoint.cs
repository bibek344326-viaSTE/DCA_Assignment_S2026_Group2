using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

[Route("api/events/{eventId:guid}/guest-status")]
public sealed class GetEventGuestStatusOverviewEndpoint(IQueryDispatcher queryDispatcher, IObjectMapper mapper)
    : ApiEndpointWithoutRequest<EventGuestStatusOverviewResponse>
{
    [HttpGet]
    public override async Task<ActionResult<EventGuestStatusOverviewResponse>> HandleAsync()
    {
        var answer = await queryDispatcher.DispatchAsync(new GetEventGuestStatusOverviewQuery(RouteValueReader.Guid(this, "eventId")));
        return answer is null ? NotFound() : Ok(mapper.Map<EventGuestStatusOverviewAnswer, EventGuestStatusOverviewResponse>(answer));
    }
}
