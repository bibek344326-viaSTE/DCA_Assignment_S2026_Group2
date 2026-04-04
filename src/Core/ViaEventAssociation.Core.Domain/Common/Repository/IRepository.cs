using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.Repository;

public interface IRepository<TEntity, TId>
{
    Task<Result<None>> AddAsync(TEntity aggregate);

    Task<Result<TEntity>> GetByIdAsync(TId id);
}