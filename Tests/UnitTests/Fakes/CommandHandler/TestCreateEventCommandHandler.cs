using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes.CommandHandler;

public class TestCreateEventCommandHandler: ICommandHandler<CreateEventCommand>
{
    public int CallCount { get; private set; }

    public Task<Result> HandleAsync(CreateEventCommand command)
    {
        CallCount++;
        return Task.FromResult<Result>(Result.Success());
    }
}