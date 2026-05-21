using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

[Route("api/events/editing-overview")]
public sealed class GetEventEditingOverviewEndpoint(IQueryDispatcher queryDispatcher, IObjectMapper mapper)
    : ApiEndpointWithoutRequest<EventEditingOverviewResponse>
{
    [HttpGet]
    public override async Task<ActionResult<EventEditingOverviewResponse>> HandleAsync()
    {
        var answer = await queryDispatcher.DispatchAsync(new GetEventEditingOverviewQuery());
        return Ok(mapper.Map<EventEditingOverviewAnswer, EventEditingOverviewResponse>(answer));
    }
}
