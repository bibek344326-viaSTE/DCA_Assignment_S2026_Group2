using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace IntegrationTests.Presentation;

internal sealed class FakeDispatcher : IDispatcher
{
    private readonly Func<object, Task<Result>> _handler;

    public List<object> DispatchedCommands { get; } = [];

    public FakeDispatcher(Result result)
        : this(_ => Task.FromResult(result))
    {
    }

    public FakeDispatcher(Func<object, Task<Result>> handler)
    {
        _handler = handler;
    }

    public async Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        DispatchedCommands.Add(command!);
        return await _handler(command!);
    }
}
