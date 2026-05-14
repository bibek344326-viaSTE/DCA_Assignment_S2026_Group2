namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Guests;

public sealed record RegisterGuestRequest(
    string? Email,
    string? FirstName,
    string? LastName,
    string? ProfilePictureUrl);
