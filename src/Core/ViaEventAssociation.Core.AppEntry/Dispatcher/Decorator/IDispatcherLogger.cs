using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;

public interface IDispatcherLogger
{
    void LogCommandStarted(string commandName);
    void LogCommandSucceeded(string commandName, long executionTimeMs);
    void LogCommandFailed(string commandName, Error error, long executionTimeMs);
    void LogCommandException(string commandName, Exception exception, long executionTimeMs);
}
