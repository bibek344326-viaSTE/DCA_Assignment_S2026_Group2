using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher;

public class CommandDispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        var handler = serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;
        if (handler is null)
        {
            throw new InvalidOperationException($"Handler not found {typeof(TCommand).Name}");
        }

        return handler.HandleAsync(command);
    }
}
