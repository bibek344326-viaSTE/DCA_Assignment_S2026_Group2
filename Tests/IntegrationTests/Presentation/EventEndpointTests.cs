using System.Net;
using System.Net.Http.Json;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Common;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Guests;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

namespace IntegrationTests.Presentation;

public class EventEndpointTests
{
    [Fact]
    public async Task CreateEvent_ReturnsCreated_WhenDispatcherSucceeds()
    {
        var dispatcher = new FakeDispatcher(Result.Success());
        await using var factory = new PresentationWebApplicationFactory(dispatcher: dispatcher);
        using var client = factory.CreateClient();

        var response = await client.PostAsync("/api/events", content: null, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CreateEventResponse>(TestContext.Current.CancellationToken);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.EventId);
        Assert.Single(dispatcher.DispatchedCommands);
    }

    [Fact]
    public async Task UpdateTitle_ReturnsBadRequest_WhenTitleIsInvalid()
    {
        await using var factory = new PresentationWebApplicationFactory(dispatcher: new FakeDispatcher(Result.Success()));
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/api/events/{Guid.NewGuid()}/title",
            new UpdateEventTitleRequest("x"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        Assert.Contains(body!.Errors, error => error.Code == "TITLE_TOO_SHORT");
    }

    [Fact]
    public async Task UpdateTitle_ReturnsServerError_WhenDispatcherThrows()
    {
        var dispatcher = new FakeDispatcher(_ => throw new InvalidOperationException("Dispatcher failed."));
        await using var factory = new PresentationWebApplicationFactory(dispatcher: dispatcher);
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            $"/api/events/{Guid.NewGuid()}/title",
            new UpdateEventTitleRequest("Valid title"),
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>(TestContext.Current.CancellationToken);
        Assert.Contains(body!.Errors, error => error.Code == "UNEXPECTED_ERROR");
    }

    [Fact]
    public async Task BrowseUpcomingEvents_MapsQueryAndReturnsAnswer()
    {
        var answer = new BrowseUpcomingEventsAnswer(
            PageNumber: 2,
            PageSize: 5,
            TotalItems: 8,
            TotalPages: 2,
            Events:
            [
                new UpcomingEventSummary(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    new DateTime(2026, 6, 1, 18, 0, 0),
                    "Friday Bar",
                    "Social event",
                    12,
                    25,
                    "public")
            ]);

        var queryDispatcher = new FakeQueryDispatcher(answer);
        await using var factory = new PresentationWebApplicationFactory(queryDispatcher: queryDispatcher);
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/events/upcoming?searchText=Friday&pageNumber=2&pageSize=5", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<BrowseUpcomingEventsResponse>(TestContext.Current.CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(2, body.PageNumber);
        Assert.Single(body.Events);
        var query = Assert.IsType<BrowseUpcomingEventsQuery>(queryDispatcher.LastQuery);
        Assert.Equal("Friday", query.SearchText);
        Assert.Equal(2, query.PageNumber);
        Assert.Equal(5, query.PageSize);
    }

    [Theory]
    [MemberData(nameof(CommandEndpointCases))]
    public async Task CommandEndpoints_DispatchExpectedCommand(
        HttpMethod method,
        string route,
        object? body,
        Type expectedCommandType)
    {
        var dispatcher = new FakeDispatcher(Result.Success());
        await using var factory = new PresentationWebApplicationFactory(dispatcher: dispatcher);
        using var client = factory.CreateClient();

        using var request = new HttpRequestMessage(method, route);
        if (body is not null)
            request.Content = JsonContent.Create(body);

        var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        Assert.True(response.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.Created);
        Assert.Contains(dispatcher.DispatchedCommands, command => command.GetType() == expectedCommandType);
    }

    [Theory]
    [MemberData(nameof(QueryEndpointCases))]
    public async Task QueryEndpoints_DispatchExpectedQuery(
        string route,
        object answer,
        Type expectedQueryType,
        HttpStatusCode expectedStatusCode)
    {
        var queryDispatcher = new FakeQueryDispatcher(answer);
        await using var factory = new PresentationWebApplicationFactory(queryDispatcher: queryDispatcher);
        using var client = factory.CreateClient();

        var response = await client.GetAsync(route, TestContext.Current.CancellationToken);

        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.IsType(expectedQueryType, queryDispatcher.LastQuery);
    }

    public static TheoryData<HttpMethod, string, object?, Type> CommandEndpointCases()
    {
        var eventId = Guid.NewGuid();
        return new TheoryData<HttpMethod, string, object?, Type>
        {
            { HttpMethod.Put, $"/api/events/{eventId}/description", new UpdateEventDescriptionRequest("Description"), typeof(UpdateDescriptionCommand) },
            { HttpMethod.Put, $"/api/events/{eventId}/time", new UpdateEventTimeRequest(new DateTime(2027, 8, 25, 19, 0, 0), new DateTime(2027, 8, 25, 21, 0, 0)), typeof(UpdateTimeCommand) },
            { HttpMethod.Put, $"/api/events/{eventId}/max-guests", new SetMaxGuestsRequest(10), typeof(SetMaxGuestsCommand) },
            { HttpMethod.Put, $"/api/events/{eventId}/public", null, typeof(MakeEventPublicCommand) },
            { HttpMethod.Put, $"/api/events/{eventId}/private", null, typeof(MakeEventPrivateCommand) },
            { HttpMethod.Put, $"/api/events/{eventId}/ready", null, typeof(ReadyEventCommand) },
            { HttpMethod.Put, $"/api/events/{eventId}/activate", null, typeof(ActivateEventCommand) },
            { HttpMethod.Post, "/api/guests", new RegisterGuestRequest("abc@via.dk", "John", "Doe", null), typeof(RegisterGuestCommand) },
            { HttpMethod.Post, $"/api/events/{eventId}/participants", new GuestEmailRequest("abc@via.dk"), typeof(ParticipateInPublicEventCommand) },
            { HttpMethod.Post, $"/api/events/{eventId}/participants/cancel", new GuestEmailRequest("abc@via.dk"), typeof(CancelParticipationCommand) },
            { HttpMethod.Post, $"/api/events/{eventId}/invitations", new GuestEmailRequest("abc@via.dk"), typeof(InviteGuestCommand) },
            { HttpMethod.Post, $"/api/events/{eventId}/invitations/accept", new GuestEmailRequest("abc@via.dk"), typeof(AcceptInvitationCommand) },
            { HttpMethod.Post, $"/api/events/{eventId}/invitations/decline", new GuestEmailRequest("abc@via.dk"), typeof(DeclineInvitationCommand) }
        };
    }

    public static TheoryData<string, object, Type, HttpStatusCode> QueryEndpointCases()
    {
        var eventId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        return new TheoryData<string, object, Type, HttpStatusCode>
        {
            {
                $"/api/guests/{guestId}/profile",
                new PersonalProfileAnswer(guestId, "John", "Doe", "abc@via.dk", "https://example.com/profile.jpg", 0, 0, [], []),
                typeof(GetPersonalProfileQuery),
                HttpStatusCode.OK
            },
            {
                $"/api/events/{eventId}?guestOffset=0&guestPageSize=9",
                new SingleEventAnswer(eventId, "Title", "Description", "Auditorium", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(2), "public", 0, 10, 0, 9, 0, []),
                typeof(GetSingleEventQuery),
                HttpStatusCode.OK
            },
            {
                "/api/events/editing-overview",
                new EventEditingOverviewAnswer([], [], []),
                typeof(GetEventEditingOverviewQuery),
                HttpStatusCode.OK
            },
            {
                $"/api/events/{eventId}/guest-status",
                new EventGuestStatusOverviewAnswer(eventId, "Title", [], [], [], [], [], []),
                typeof(GetEventGuestStatusOverviewQuery),
                HttpStatusCode.OK
            }
        };
    }
}
