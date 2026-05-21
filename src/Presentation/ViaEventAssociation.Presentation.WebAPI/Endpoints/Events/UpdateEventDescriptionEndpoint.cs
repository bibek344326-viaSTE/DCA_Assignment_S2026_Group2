using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events/{eventId:guid}/description")]
public sealed class UpdateEventDescriptionEndpoint(IDispatcher dispatcher)
    : ApiEndpoint<UpdateEventDescriptionRequest, object>
{
    [HttpPut]
    public override async Task<ActionResult<object>> HandleAsync([FromBody] UpdateEventDescriptionRequest request)
        => await CommandEndpoint.DispatchAsync(this, dispatcher,
            UpdateDescriptionCommand.Create(RouteValueReader.Guid(this, "eventId"), request.Description));
}
