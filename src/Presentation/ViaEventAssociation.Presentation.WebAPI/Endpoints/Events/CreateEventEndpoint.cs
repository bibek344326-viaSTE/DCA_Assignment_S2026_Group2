using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Events;

[Route("api/events")]
public sealed class CreateEventEndpoint(IDispatcher dispatcher)
    : ApiEndpointWithoutRequest<CreateEventResponse>
{
    [HttpPost]
    public override async Task<ActionResult<CreateEventResponse>> HandleAsync()
    {
        try
        {
            var commandResult = CreateEventCommand.Create();
            if (commandResult is Failure<CreateEventCommand>)
                return EndpointResults.Failure(this, commandResult);

            var dispatchResult = await dispatcher.DispatchAsync(commandResult.Payload!);
            if (dispatchResult.IsFailure)
                return EndpointResults.Failure(this, dispatchResult);

            var response = new CreateEventResponse(commandResult.Payload!.Id.Value);
            return Created($"/api/events/{response.EventId}", response);
        }
        catch (Exception exception)
        {
            return EndpointResults.Exception(this, exception);
        }
    }
}
