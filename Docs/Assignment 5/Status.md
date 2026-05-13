# Assignment 5 - Commands and Handlers Status

## Application Structure

| Requirement | Status | Notes |
| --- | --- | --- |
| Application project | Done | `ViaEventAssociation.Core.Application` contains command handlers. |
| App entry project | Done | `ViaEventAssociation.Core.AppEntry` contains commands, dispatcher abstractions, and `ICommandHandler<TCommand>`. |
| Repository interfaces | Done | Aggregate repository interfaces exist for events, guests, and locations. |
| Unit of work | Done | `IUnitOfWork.SaveChangesAsync()` exists in the Domain project. |
| Fake repositories and unit of work | Done | Test fakes exist in `Tests/UnitTests/Fakes`. |

## Must-Have Use Cases

| ID | Use Case | Command | Handler | Tests |
| --- | --- | --- | --- | --- |
| UC1 | Create event | Done | Done | Done |
| UC2 | Update event title | Done | Done | Done |
| UC3 | Update event description | Done | Done | Done |
| UC4 | Update event time | Done | Done | Done |
| UC5 | Make event public | Done | Done | Done |
| UC6 | Make event private | Done | Done | Done |
| UC7 | Set max guests | Done | Done | Done |
| UC8 | Ready event | Done | Done | Done |
| UC9 | Activate event | Done | Done | Done |
| UC10 | Register guest account | Done | Done | Done |
| UC11 | Participate in public event | Done | Done | Done |
| UC12 | Cancel participation | Done | Done | Done |
| UC13 | Invite guest | Done | Done | Done |
| UC14 | Accept invitation | Done | Done | Done |
| UC15 | Decline invitation | Done | Done | Done |

## Verification

```powershell
dotnet test Tests\UnitTests\UnitTests.csproj --no-restore
```

Result: `256` passed, `0` failed.
