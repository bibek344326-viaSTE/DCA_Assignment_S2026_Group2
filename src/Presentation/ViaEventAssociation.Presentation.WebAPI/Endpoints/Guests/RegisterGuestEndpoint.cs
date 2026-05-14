using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Guests;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Guests;

[Route("api/guests")]
public sealed class RegisterGuestEndpoint(IDispatcher dispatcher)
    : ApiEndpoint<RegisterGuestRequest, RegisterGuestResponse>
{
    [HttpPost]
    public override async Task<ActionResult<RegisterGuestResponse>> HandleAsync([FromBody] RegisterGuestRequest request)
    {
        try
        {
            var commandResult = RegisterGuestCommand.Create(
                request.Email,
                request.FirstName,
                request.LastName,
                request.ProfilePictureUrl);

            if (commandResult is Failure<RegisterGuestCommand>)
                return EndpointResults.Failure(this, commandResult);

            var result = await dispatcher.DispatchAsync(commandResult.Payload!);
            if (result.IsFailure)
                return EndpointResults.Failure(this, result);

            var response = new RegisterGuestResponse(commandResult.Payload!.Email);
            return Created($"/api/guests/{response.Email}", response);
        }
        catch (Exception exception)
        {
            return EndpointResults.Exception(this, exception);
        }
    }
}
