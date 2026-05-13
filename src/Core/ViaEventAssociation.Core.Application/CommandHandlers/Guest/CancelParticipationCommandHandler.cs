using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;
using DomainGuest = ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate.Guest;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Guest;

public class CancelParticipationCommandHandler(
    IEventRepository eventRepository,
    IGuestRepository guestRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelParticipationCommand>
{
    public async Task<Result> HandleAsync(CancelParticipationCommand command)
    {
        var eventResult = await eventRepository.GetByIdAsync(command.Id);
        if (eventResult is Failure<EventRoot> eventFailure)
            return Result.Failure<None>(eventFailure.Errors);

        var @event = eventResult.Payload;
        if (@event is null)
            return Result.Failure<None>(Error.EventNotFound);

        var guestResult = await guestRepository.GetByIdAsync(command.GuestEmail);
        if (guestResult is Failure<DomainGuest> guestFailure)
            return Result.Failure<None>(guestFailure.Errors);

        if (guestResult.Payload is null)
            return Result.Failure<None>(Error.GuestNotFound);

        var cancelResult = @event.RemoveParticipant(command.GuestEmail);
        if (cancelResult is Failure<None> cancelFailure)
            return Result.Failure<None>(cancelFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
