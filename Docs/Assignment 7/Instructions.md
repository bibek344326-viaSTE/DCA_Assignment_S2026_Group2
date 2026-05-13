# Assignment 7 - Persistence Instructions Summary

## Goal

Implement the persistence part of the infrastructure layer using EF Core and SQLite.

## Required Work

| Requirement | Where it is implemented |
| --- | --- |
| Infrastructure persistence project | `src/Infrastructure/ViaEventAssociation.Infrastructure.EfcDmPersistence` |
| EF Core packages | `ViaEventAssociation.Infrastructure.EfcDmPersistence.csproj` |
| Reference to Domain | Persistence project references `ViaEventAssociation.Core.Domain` |
| DbContext | `Contexts/DmContext.cs` |
| Design-time context factory | `Contexts/DmContextDesignTimeFactory.cs` |
| EF configurations | `Configurations/EventRootConfiguration.cs`, `GuestConfiguration.cs`, `EventLocationConfiguration.cs` |
| Generic repository base | `Repositories/EfcRepository.cs` |
| Specific repositories | `EventRepository.cs`, `GuestRepository.cs`, `LocationRepository.cs` |
| Unit of Work implementation | `UnitOfWork/EfcUnitOfWork.cs` |
| Integration test project | `Tests/IntegrationTests` |
| Repository integration tests | `Tests/IntegrationTests/Persistence/RepositoryTests` |
| UC1-UC15 persistence tests | `Tests/IntegrationTests/Persistence/UseCasePersistenceTests.cs` |

## Verification Commands

```powershell
dotnet test Tests\IntegrationTests\IntegrationTests.csproj
dotnet test Tests\UnitTests\UnitTests.csproj --no-restore
```

Expected result:

- Integration tests: `20 passed, 0 failed`
- Unit tests: `256 passed, 0 failed`

## Note

The optional soft-delete challenge is not implemented.
