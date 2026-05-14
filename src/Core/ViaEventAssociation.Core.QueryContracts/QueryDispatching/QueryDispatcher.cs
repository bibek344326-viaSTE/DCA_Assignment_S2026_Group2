namespace ViaEventAssociation.Core.QueryContracts;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TAnswer));
        var handler = serviceProvider.GetService(handlerType);

        if (handler is null)
            throw new InvalidOperationException($"No query handler registered for {query.GetType().Name}.");

        return ((dynamic)handler).HandleAsync((dynamic)query);
    }
}
