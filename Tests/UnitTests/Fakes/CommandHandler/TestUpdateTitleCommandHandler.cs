using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Fakes.CommandHandler;

public class TestUpdateTitleCommandHandler : ICommandHandler<UpdateTitleCommand>
{
    public int CallCount { get; private set; }

    public Task<Result> HandleAsync(UpdateTitleCommand command)
    {
        CallCount++;
        return Task.FromResult<Result>(Result.Success());
    }
}

