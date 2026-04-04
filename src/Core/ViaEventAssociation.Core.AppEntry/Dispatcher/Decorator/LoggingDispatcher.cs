using System.Diagnostics;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;

public class LoggingDispatcher : IDispatcher
{
    private readonly IDispatcher _innerDispatcher;
    private readonly IDispatcherLogger _logger;

    public LoggingDispatcher(IDispatcher innerDispatcher, IDispatcherLogger logger)
    {
        _innerDispatcher = innerDispatcher;
        _logger = logger;
    }

    public async Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        var commandName = typeof(TCommand).Name;
        var stopwatch = Stopwatch.StartNew();

        // Log command dispatch start
        _logger.LogCommandStarted(commandName);

        try
        {
            // Call the inner dispatcher
            var result = await _innerDispatcher.DispatchAsync(command);

            stopwatch.Stop();
            var executionTime = stopwatch.ElapsedMilliseconds;

            // Log result based on success/failure
            if (result.IsSuccess)
            {
                _logger.LogCommandSucceeded(commandName, executionTime);
            }
            else if (result is Result<None> resultNone && resultNone.IsFailure)
            {
                _logger.LogCommandFailed(commandName, resultNone.Error, executionTime);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var executionTime = stopwatch.ElapsedMilliseconds;
            _logger.LogCommandException(commandName, ex, executionTime);
            throw;
        }
    }
}
