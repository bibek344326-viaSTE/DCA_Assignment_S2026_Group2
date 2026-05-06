using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Event;

public class ReadyEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ReadyEventCommand>
{
    public async Task<Result> HandleAsync(ReadyEventCommand command)
    {
        var getResult = await eventRepository.GetByIdAsync(command.Id);
        if (getResult is Failure<EventRoot> getFailure)
            return Result.Failure<None>(getFailure.Errors);

        var @event = getResult.Payload;
        if (@event is null)
            return Result.Failure<None>(Error.EventNotFound);

        var readyResult = @event.Ready();
        if (readyResult is Failure<None> readyFailure)
            return Result.Failure<None>(readyFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
