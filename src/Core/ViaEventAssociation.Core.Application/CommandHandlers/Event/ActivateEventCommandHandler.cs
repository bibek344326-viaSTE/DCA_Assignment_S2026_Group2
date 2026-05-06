using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Event;

public class ActivateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ActivateEventCommand>
{
    public async Task<Result> HandleAsync(ActivateEventCommand command)
    {
        var getResult = await eventRepository.GetByIdAsync(command.Id);
        if (getResult is Failure<EventRoot> getFailure)
            return Result.Failure<None>(getFailure.Errors);

        var @event = getResult.Payload;
        if (@event is null)
            return Result.Failure<None>(Error.EventNotFound);

        var activateResult = @event.Activate();
        if (activateResult is Failure<None> activateFailure)
            return Result.Failure<None>(activateFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
