using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Event;

public class MakeEventPrivateCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<MakeEventPrivateCommand>
{
    public async Task<Result> HandleAsync(MakeEventPrivateCommand command)
    {
        var getResult = await eventRepository.GetByIdAsync(command.Id);
        if (getResult is Failure<EventRoot> getFailure)
            return Result.Failure<None>(getFailure.Errors);

        if (getResult.Payload is null)
            return Result.Failure<None>(new Error("EVENT_NOT_FOUND", "Event not found."));

        var @event = getResult.Payload;

        var makePrivateResult = @event.MakePrivate();
        if (makePrivateResult is Failure<None> failure)
            return Result.Failure<None>(failure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}