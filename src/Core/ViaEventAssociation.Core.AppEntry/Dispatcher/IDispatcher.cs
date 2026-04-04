using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher;

public interface IDispatcher
{
    public Task<Result> DispatchAsync<TCommand>(TCommand command);
}