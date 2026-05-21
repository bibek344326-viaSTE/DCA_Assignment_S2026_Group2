using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

public static class CommandEndpoint
{
    public static async Task<ActionResult> DispatchAsync<TCommand>(
        ControllerBase controller,
        IDispatcher dispatcher,
        Result<TCommand> commandResult)
    {
        try
        {
            if (commandResult is Failure<TCommand>)
                return EndpointResults.Failure(controller, commandResult);

            var result = await dispatcher.DispatchAsync(commandResult.Payload!);
            return result.IsSuccess
                ? controller.NoContent()
                : EndpointResults.Failure(controller, result);
        }
        catch (Exception exception)
        {
            return EndpointResults.Exception(controller, exception);
        }
    }
}
