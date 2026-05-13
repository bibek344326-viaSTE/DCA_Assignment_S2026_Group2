using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.UnitOfWork;

public class EfcUnitOfWork(DmContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
