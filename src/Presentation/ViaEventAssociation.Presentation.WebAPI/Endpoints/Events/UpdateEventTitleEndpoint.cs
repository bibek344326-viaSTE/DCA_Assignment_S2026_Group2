using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events/{eventId:guid}/title")]
public sealed class UpdateEventTitleEndpoint(IDispatcher dispatcher)
    : ApiEndpoint<UpdateEventTitleRequest, object>
{
    [HttpPut]
    public override async Task<ActionResult<object>> HandleAsync([FromBody] UpdateEventTitleRequest request)
    {
        try
        {
            var eventId = Guid.Parse((string)RouteData.Values["eventId"]!);
            var commandResult = UpdateTitleCommand.Create(eventId, request.Title ?? string.Empty);
            if (commandResult is Failure<UpdateTitleCommand>)
                return EndpointResults.Failure(this, commandResult);

            var dispatchResult = await dispatcher.DispatchAsync(commandResult.Payload!);
            if (dispatchResult.IsFailure)
                return EndpointResults.Failure(this, dispatchResult);

            return NoContent();
        }
        catch (Exception exception)
        {
            return EndpointResults.Exception(this, exception);
        }
    }
}
