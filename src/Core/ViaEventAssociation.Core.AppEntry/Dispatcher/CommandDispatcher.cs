using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher;

public class CommandDispatcher: IDispatcher 
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        Type handlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(TCommand));
        dynamic handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found {typeof(TCommand).Name}");
        }
        
        return handler.HandleAsync((dynamic)command);
    }
}