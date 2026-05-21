using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

[Route("api/events/{eventId:guid}")]
public sealed class GetSingleEventEndpoint(IQueryDispatcher queryDispatcher, IObjectMapper mapper)
    : ApiEndpointWithoutRequest<SingleEventResponse>
{
    [HttpGet]
    public override async Task<ActionResult<SingleEventResponse>> HandleAsync()
    {
        var offset = int.TryParse(Request.Query["guestOffset"], out var parsedOffset) ? parsedOffset : 0;
        var pageSize = int.TryParse(Request.Query["guestPageSize"], out var parsedPageSize) ? parsedPageSize : 9;
        var answer = await queryDispatcher.DispatchAsync(new GetSingleEventQuery(RouteValueReader.Guid(this, "eventId"), offset, pageSize));
        return answer is null ? NotFound() : Ok(mapper.Map<SingleEventAnswer, SingleEventResponse>(answer));
    }
}
