using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events/{eventId:guid}/private")]
public sealed class MakeEventPrivateEndpoint(IDispatcher dispatcher)
    : ApiEndpointWithoutRequest<object>
{
    [HttpPut]
    public override async Task<ActionResult<object>> HandleAsync()
        => await CommandEndpoint.DispatchAsync(this, dispatcher,
            MakeEventPrivateCommand.Create(RouteValueReader.Guid(this, "eventId")));
}
