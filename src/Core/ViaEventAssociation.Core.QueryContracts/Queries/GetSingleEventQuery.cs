namespace ViaEventAssociation.Core.QueryContracts.Queries;

public sealed record GetSingleEventQuery(
    Guid EventId,
    int GuestOffset,
    int GuestPageSize) : IQuery<SingleEventAnswer?>;

public sealed record SingleEventAnswer(
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
    IReadOnlyList<EventGuestSummary> Guests);

public sealed record EventGuestSummary(
    Guid GuestId,
    string FirstName,
    string LastName,
    string ProfilePictureUrl);
