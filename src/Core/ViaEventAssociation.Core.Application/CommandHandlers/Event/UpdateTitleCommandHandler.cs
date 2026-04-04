using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Event;

public class UpdateTitleCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateTitleCommand>
{
    public async Task<Result> HandleAsync(UpdateTitleCommand command)
    {
        var getResult = await eventRepository.GetByIdAsync(command.Id);
        if (getResult is Failure<EventRoot> getFailure)
            return Result.Failure<None>(getFailure.Errors);

        if (getResult.Payload is null)
            return Result.Failure<None>(new Error("EVENT_NOT_FOUND", "Event not found."));

        var @event = getResult.Payload;

        var updateResult = @event.UpdateTitle(command.title);
        if (updateResult is Failure<None> updateFailure)
            return Result.Failure<None>(updateFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}