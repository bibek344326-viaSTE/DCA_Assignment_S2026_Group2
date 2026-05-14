namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Common;

public sealed record ErrorResponse(IReadOnlyList<ApiError> Errors);

public sealed record ApiError(string Code, string Message);
