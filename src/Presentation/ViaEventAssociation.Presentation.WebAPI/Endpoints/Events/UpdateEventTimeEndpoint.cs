using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events/{eventId:guid}/time")]
public sealed class UpdateEventTimeEndpoint(IDispatcher dispatcher)
    : ApiEndpoint<UpdateEventTimeRequest, object>
{
    [HttpPut]
    public override async Task<ActionResult<object>> HandleAsync([FromBody] UpdateEventTimeRequest request)
        => await CommandEndpoint.DispatchAsync(this, dispatcher,
            UpdateTimeCommand.Create(RouteValueReader.Guid(this, "eventId"), request.StartDateTime, request.EndDateTime));
}
