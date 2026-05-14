using Microsoft.AspNetCore.Mvc;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

public static class RouteValueReader
{
    public static Guid Guid(ControllerBase controller, string name)
        => System.Guid.Parse(controller.RouteData.Values[name]?.ToString()
            ?? throw new InvalidOperationException($"Missing route value '{name}'."));
}
