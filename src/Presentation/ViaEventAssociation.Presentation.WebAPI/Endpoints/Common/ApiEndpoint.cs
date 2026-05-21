using Microsoft.AspNetCore.Mvc;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

[ApiController]
public abstract class ApiEndpoint<TRequest, TResponse> : ControllerBase
    where TRequest : notnull
{
    public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
}

[ApiController]
public abstract class ApiEndpointWithoutRequest<TResponse> : ControllerBase
{
    public abstract Task<ActionResult<TResponse>> HandleAsync();
}
