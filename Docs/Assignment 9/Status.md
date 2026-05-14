# Assignment 9 - Presentation Status

## Implemented

| Requirement | Status | Notes |
| --- | --- | --- |
| Presentation Web API project | Done | `src/Presentation/ViaEventAssociation.Presentation.WebAPI` uses controller-based endpoints. |
| REPR fluent endpoint base | Done | `Endpoints/Common/ApiEndpoint.cs` provides request/response endpoint base classes. |
| Object mapper project | Done | `src/Core/Tools/ViaEventAssociation.Core.Tools.ObjectMapper` contains `IObjectMapper`, `IMapping`, implementation, and DI extension. |
| Object mapper tests | Done | Unit tests cover default property mapping and explicit registered mappings. |
| Command endpoints | Done | All UC1-UC15 command use cases are exposed through HTTP endpoints. |
| Query endpoints | Done | All Assignment 8 query views are exposed through HTTP endpoints. |
| Endpoint integration tests | Done | Tests cover command success, input failure, exception handling, and query mapping/response behavior. |
| Dependency registration | Done | `Program.cs` registers command handlers, dispatchers, persistence contexts, query handlers, object mapper, and presentation mappings. |

## Verification

```powershell
dotnet test ViaEventAssociation.slnx --no-restore
```

Current result:

- Unit tests: 259 passed, 0 failed.
- Integration tests: 47 passed, 0 failed.

## Command Endpoints

| Use case | Endpoint |
| --- | --- |
| UC1 Create event | `POST /api/events` |
| UC2 Update title | `PUT /api/events/{eventId}/title` |
| UC3 Update description | `PUT /api/events/{eventId}/description` |
| UC4 Update time | `PUT /api/events/{eventId}/time` |
| UC5 Make public | `PUT /api/events/{eventId}/public` |
| UC6 Make private | `PUT /api/events/{eventId}/private` |
| UC7 Set max guests | `PUT /api/events/{eventId}/max-guests` |
| UC8 Ready event | `PUT /api/events/{eventId}/ready` |
| UC9 Activate event | `PUT /api/events/{eventId}/activate` |
| UC10 Register guest | `POST /api/guests` |
| UC11 Participate in public event | `POST /api/events/{eventId}/participants` |
| UC12 Cancel participation | `POST /api/events/{eventId}/participants/cancel` |
| UC13 Invite guest | `POST /api/events/{eventId}/invitations` |
| UC14 Accept invitation | `POST /api/events/{eventId}/invitations/accept` |
| UC15 Decline invitation | `POST /api/events/{eventId}/invitations/decline` |

## Query Endpoints

| Assignment 8 view | Endpoint |
| --- | --- |
| Browse upcoming events | `GET /api/events/upcoming` |
| Personal profile | `GET /api/guests/{guestId}/profile` |
| Single event details | `GET /api/events/{eventId}` |
| Event editing overview | `GET /api/events/editing-overview` |
| Event guest status overview | `GET /api/events/{eventId}/guest-status` |

## Notes

The implementation intentionally keeps the Web API endpoints thin. Domain rules, command handlers, persistence, and query handlers remain covered by the existing unit and integration tests.

Optional challenges such as moving the composition root to a separate project, global exception middleware, and Swagger grouping are not implemented.
