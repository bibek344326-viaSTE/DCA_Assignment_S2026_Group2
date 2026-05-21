using ViaEventAssociation.Core.QueryContracts;

namespace IntegrationTests.Presentation;

internal sealed class FakeQueryDispatcher : IQueryDispatcher
{
    private readonly Func<object, object?> _handler;

    public object? LastQuery { get; private set; }

    public FakeQueryDispatcher(object? answer)
        : this(_ => answer)
    {
    }

    public FakeQueryDispatcher(Func<object, object?> handler)
    {
        _handler = handler;
    }

    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        LastQuery = query;
        return Task.FromResult((TAnswer)_handler(query)!);
    }
}
