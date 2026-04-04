using ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Dispatcher.Decorator;

/// <summary>
/// Mock implementation of IDispatcherLogger for testing.
/// Tracks all log calls so we can assert on them in tests.
/// </summary>
public class TestDispatcherLogger : IDispatcherLogger
{
    public List<string> LogMessages { get; private set; } = new();
    public List<(string CommandName, Error Error, long ExecutionTimeMs)> FailedCommands { get; private set; } = new();
    public List<(string CommandName, Exception Exception, long ExecutionTimeMs)> ExceptionCommands { get; private set; } = new();
    public List<(string CommandName, long ExecutionTimeMs)> SuccessfulCommands { get; private set; } = new();

    public void LogCommandStarted(string commandName)
    {
        LogMessages.Add($"[STARTED] {commandName}");
    }

    public void LogCommandSucceeded(string commandName, long executionTimeMs)
    {
        LogMessages.Add($"[SUCCESS] {commandName} - {executionTimeMs}ms");
        SuccessfulCommands.Add((commandName, executionTimeMs));
    }

    public void LogCommandFailed(string commandName, Error error, long executionTimeMs)
    {
        LogMessages.Add($"[FAILED] {commandName}: {error.Code} - {executionTimeMs}ms");
        FailedCommands.Add((commandName, error, executionTimeMs));
    }

    public void LogCommandException(string commandName, Exception exception, long executionTimeMs)
    {
        LogMessages.Add($"[EXCEPTION] {commandName}: {exception.GetType().Name} - {executionTimeMs}ms");
        ExceptionCommands.Add((commandName, exception, executionTimeMs));
    }
}

