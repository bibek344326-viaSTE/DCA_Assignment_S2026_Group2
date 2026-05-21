namespace ViaEventAssociation.Core.QueryContracts;

public interface IQueryHandler<in TQuery, TAnswer>
    where TQuery : IQuery<TAnswer>
{
    Task<TAnswer> HandleAsync(TQuery query);
}
