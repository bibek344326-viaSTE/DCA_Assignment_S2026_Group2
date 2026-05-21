using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events/{eventId:guid}/max-guests")]
public sealed class SetMaxGuestsEndpoint(IDispatcher dispatcher)
    : ApiEndpoint<SetMaxGuestsRequest, object>
{
    [HttpPut]
    public override async Task<ActionResult<object>> HandleAsync([FromBody] SetMaxGuestsRequest request)
        => await CommandEndpoint.DispatchAsync(this, dispatcher,
            SetMaxGuestsCommand.Create(RouteValueReader.Guid(this, "eventId"), request.MaxGuests));
}
