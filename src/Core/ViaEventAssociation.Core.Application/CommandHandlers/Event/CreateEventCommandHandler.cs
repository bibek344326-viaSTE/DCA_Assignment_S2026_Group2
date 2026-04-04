using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Event;

internal class CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand>
{
    public async Task<Result> HandleAsync(CreateEventCommand command)
    {
        var existing = await eventRepository.GetByIdAsync(command.Id);
        if (existing.Payload is not null)
            return Result.Failure<None>(new Error("EVENT_ALREADY_EXISTS", "An event with the provided id already exists."));

        var newEvent = EventRoot.Create(command.Id);

        var addResult = await eventRepository.AddAsync(newEvent);
        if (addResult is Failure<None> addFailure)
            return Result.Failure<None>(addFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();

    }
}