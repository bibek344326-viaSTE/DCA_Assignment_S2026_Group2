using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries;

[Route("api/guests/{guestId:guid}/profile")]
public sealed class GetPersonalProfileEndpoint(IQueryDispatcher queryDispatcher, IObjectMapper mapper)
    : ApiEndpointWithoutRequest<PersonalProfileResponse>
{
    [HttpGet]
    public override async Task<ActionResult<PersonalProfileResponse>> HandleAsync()
    {
        var answer = await queryDispatcher.DispatchAsync(new GetPersonalProfileQuery(RouteValueReader.Guid(this, "guestId")));
        return answer is null ? NotFound() : Ok(mapper.Map<PersonalProfileAnswer, PersonalProfileResponse>(answer));
    }
}
