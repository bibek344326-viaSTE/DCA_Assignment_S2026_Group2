namespace ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

public sealed record SingleEventResponse(
    Guid EventId,
    string Title,
    string Description,
    string LocationName,
    DateTime Start,
    DateTime End,
    string Visibility,
    int AttendeeCount,
    int MaxGuests,
    int GuestOffset,
    int GuestPageSize,
    int TotalGuests,
    IReadOnlyList<EventGuestResponse> Guests);

public sealed record EventGuestResponse(
    Guid GuestId,
    string FirstName,
    string LastName,
    string ProfilePictureUrl);
