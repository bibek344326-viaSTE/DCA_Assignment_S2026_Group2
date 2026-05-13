using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.Domain.Common.Repository;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;

namespace ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;

public abstract class EfcRepository<TEntity, TId>(DmContext context) : IRepository<TEntity, TId>
    where TEntity : class
{
    public Task<Result<None>> AddAsync(TEntity aggregate)
    {
        context.Set<TEntity>().Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public async Task<Result<TEntity>> GetByIdAsync(TId id)
    {
        var aggregate = await context.Set<TEntity>().FindAsync(id);

        return aggregate is null
            ? Result.Failure<TEntity>(new Error("AGGREGATE_NOT_FOUND", "Aggregate not found."))
            : Result.Success(aggregate);
    }
}
