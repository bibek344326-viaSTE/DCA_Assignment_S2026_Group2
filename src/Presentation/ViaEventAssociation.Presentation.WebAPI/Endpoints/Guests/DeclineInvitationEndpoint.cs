using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Common;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Guests;

[Route("api/events/{eventId:guid}/invitations/decline")]
public sealed class DeclineInvitationEndpoint(IDispatcher dispatcher)
    : ApiEndpoint<GuestEmailRequest, object>
{
    [HttpPost]
    public override async Task<ActionResult<object>> HandleAsync([FromBody] GuestEmailRequest request)
        => await CommandEndpoint.DispatchAsync(this, dispatcher,
            DeclineInvitationCommand.Create(RouteValueReader.Guid(this, "eventId"), request.GuestEmail));
}
