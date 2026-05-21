using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;
using ViaEventAssociation.Infrastructure.EfcQueries.Models;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Seed;

public static class QueryDbContextSeedExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedFromJsonDirectoryAsync(this QueryDbContext context, string directoryPath)
    {
        if (await context.Events.AnyAsync())
            return;

        var locations = await ReadJsonAsync<TmpLocation>(directoryPath, "Locations.json");
        var guests = await ReadJsonAsync<TmpGuest>(directoryPath, "Guests.json");
        var events = await ReadJsonAsync<TmpEvent>(directoryPath, "Events.json");
        var participations = await ReadJsonAsync<TmpParticipation>(directoryPath, "Participations.json");
        var invitations = await ReadJsonAsync<TmpInvitation>(directoryPath, "Invitations.json");
        var joinRequests = await ReadJsonAsync<TmpJoinRequest>(directoryPath, "JoinRequests.json");

        context.Locations.AddRange(locations.Select(location => new LocationReadModel
        {
            Id = Guid.Parse(location.Id),
            Name = location.Name,
            MaxCapacity = location.MaxCapacity,
            AvailabilityStart = location.AvailabilityStart,
            AvailabilityEnd = location.AvailabilityEnd
        }));

        context.Guests.AddRange(guests.Select(guest => new GuestReadModel
        {
            Id = Guid.Parse(guest.Id),
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            Url = guest.Url
        }));

        context.Events.AddRange(events.Select(eventData => new EventReadModel
        {
            Id = Guid.Parse(eventData.Id),
            Title = eventData.Title,
            Description = eventData.Description,
            Status = eventData.Status,
            Visibility = eventData.Visibility,
            Start = DateTime.ParseExact(eventData.Start, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            End = DateTime.ParseExact(eventData.End, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
            MaxGuests = eventData.MaxGuests,
            LocationId = Guid.Parse(eventData.LocationId)
        }));

        context.Participations.AddRange(participations
            .DistinctBy(participation => (participation.EventId, participation.GuestId))
            .Select(participation => new ParticipationReadModel
            {
                EventId = Guid.Parse(participation.EventId),
                GuestId = Guid.Parse(participation.GuestId)
            }));

        context.Invitations.AddRange(invitations
            .DistinctBy(invitation => (invitation.EventId, invitation.GuestId))
            .Select(invitation => new InvitationReadModel
            {
                EventId = Guid.Parse(invitation.EventId),
                GuestId = Guid.Parse(invitation.GuestId),
                Status = invitation.Status
            }));

        context.JoinRequests.AddRange(joinRequests
            .DistinctBy(joinRequest => (joinRequest.EventId, joinRequest.GuestId))
            .Select(joinRequest => new JoinRequestReadModel
            {
                EventId = Guid.Parse(joinRequest.EventId),
                GuestId = Guid.Parse(joinRequest.GuestId),
                Reason = joinRequest.Reason,
                Status = joinRequest.Status
            }));

        await context.SaveChangesAsync();
    }

    private static async Task<IReadOnlyList<T>> ReadJsonAsync<T>(string directoryPath, string fileName)
    {
        var filePath = Path.Combine(directoryPath, fileName);
        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<List<T>>(stream, JsonOptions) ?? [];
    }

    private sealed record TmpLocation(
        string Id,
        string Name,
        int MaxCapacity,
        string AvailabilityStart,
        string AvailabilityEnd);

    private sealed record TmpGuest(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string Url);

    private sealed record TmpEvent(
        string Id,
        string Title,
        string Description,
        string Status,
        string Visibility,
        string Start,
        string End,
        int MaxGuests,
        string LocationId);

    private sealed record TmpParticipation(string EventId, string GuestId);

    private sealed record TmpInvitation(string EventId, string GuestId, string Status);

    private sealed record TmpJoinRequest(string EventId, string GuestId, string Reason, string Status);
}
