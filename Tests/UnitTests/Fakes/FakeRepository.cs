using ViaEventAssociation.Core.Domain.Common.Repository;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes;

public class FakeRepository<TEntity, TId>(Func<TEntity, TId> idSelector) : IRepository<TEntity, TId>
    where TId : notnull
{
    public List<TEntity> Values { get; } = new();

    public Task<Result<None>> AddAsync(TEntity aggregate)
    {
        Values.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<TEntity>> GetByIdAsync(TId id)
    {
        var found = Values.FirstOrDefault(e => EqualityComparer<TId>.Default.Equals(idSelector(e), id));

        return Task.FromResult(found is null
            ? Result.Failure<TEntity>(new Error("NOT_FOUND", "Aggregate not found."))
            : Result.Success(found));
    }
}