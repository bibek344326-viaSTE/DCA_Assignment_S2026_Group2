using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

public static class EndpointResults
{
    public static ActionResult Failure(ControllerBase controller, Result result)
    {
        var errors = ExtractErrors(result)
            .Select(error => new ApiError(error.Code, error.Message))
            .ToList();

        return controller.BadRequest(new ErrorResponse(errors));
    }

    public static ActionResult Exception(ControllerBase controller, Exception exception)
        => controller.StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse([
            new ApiError("UNEXPECTED_ERROR", exception.Message)
        ]));

    private static IEnumerable<Error> ExtractErrors(Result result)
    {
        var errorsProperty = result.GetType().GetProperty("Errors");
        return errorsProperty?.GetValue(result) as IEnumerable<Error>
            ?? [new Error("REQUEST_FAILED", "The request failed.")];
    }
}
