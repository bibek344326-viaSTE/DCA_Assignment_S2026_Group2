# Assignment 7 - Persistence Status

## Infrastructure Structure

| Requirement | Status | Notes |
| --- | --- | --- |
| Infrastructure persistence project | Done | Added `ViaEventAssociation.Infrastructure.EfcDmPersistence`. |
| EF Core packages | Done | Uses `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Design`, and `Microsoft.EntityFrameworkCore.Sqlite`. |
| Domain reference | Done | Persistence project references the Domain project. |
| DbContext | Done | `DmContext` exposes `Events`, `Guests`, and `Locations` and applies configurations from the assembly. |
| Design-time context factory | Done | `DmContextDesignTimeFactory` creates a SQLite-backed context for EF tooling. |
| Entity configurations | Done | Aggregate roots, value objects, enums, private fields, and owned `Email` collections are configured. |
| Unit of Work | Done | `EfcUnitOfWork` delegates to `DmContext.SaveChangesAsync()`. |
| Repository pattern | Done | Generic `EfcRepository<TEntity, TId>` plus event, guest, and location repository implementations. |
| Integration tests | Done | Added a separate `IntegrationTests` project with SQLite-backed repository tests. |

## Persistence Coverage

| Aggregate | Repository | Integration Coverage |
| --- | --- | --- |
| EventRoot | `EventRepository` | Saves and reloads draft event state plus participants, pending invitations, and declined invitations. |
| Guest | `GuestRepository` | Saves and reloads email identity, names, and profile picture URL. |
| EventLocation | `LocationRepository` | Saves and reloads name, capacity, and availability interval. |

## Must-Have Use Case Persistence Coverage

| ID | Use Case | Persistence Test |
| --- | --- | --- |
| UC1 | Creator creates a new empty event | Done |
| UC2 | Creator updates event title | Done |
| UC3 | Creator updates event description | Done |
| UC4 | Creator updates event start/end date and time | Done |
| UC5 | Creator makes event public | Done |
| UC6 | Creator makes event private | Done |
| UC7 | Creator sets max guests | Done |
| UC8 | Creator readies event | Done |
| UC9 | Creator activates event | Done |
| UC10 | Anonymous registers guest account | Done |
| UC11 | Guest participates in public event | Done |
| UC12 | Guest cancels participation | Done |
| UC13 | Creator invites guest to event | Done |
| UC14 | Guest accepts invitation | Done |
| UC15 | Guest declines invitation | Done |

## Verification

```powershell
dotnet test Tests\IntegrationTests\IntegrationTests.csproj --no-restore
```

Result: `20` passed, `0` failed.

```powershell
dotnet test Tests\UnitTests\UnitTests.csproj --no-restore
```

Result: `256` passed, `0` failed.

## Notes

The optional soft-delete challenge is not implemented.
