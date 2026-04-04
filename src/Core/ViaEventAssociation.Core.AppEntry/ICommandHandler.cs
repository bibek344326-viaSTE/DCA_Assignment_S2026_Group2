using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry;

public interface ICommandHandler<TCommand>
{
    Task<Result> HandleAsync(TCommand command);
}
