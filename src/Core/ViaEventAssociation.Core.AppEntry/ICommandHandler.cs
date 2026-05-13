using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry;

public interface ICommandHandler<in TCommand>
{
    Task<Result> HandleAsync(TCommand command);
}
