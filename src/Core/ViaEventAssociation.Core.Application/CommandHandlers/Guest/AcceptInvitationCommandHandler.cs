using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;
using DomainGuest = ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate.Guest;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Guest;

public class AcceptInvitationCommandHandler(
    IEventRepository eventRepository,
    IGuestRepository guestRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AcceptInvitationCommand>
{
    public async Task<Result> HandleAsync(AcceptInvitationCommand command)
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

        var acceptResult = @event.AcceptInvitation(command.GuestEmail);
        if (acceptResult is Failure<None> acceptFailure)
            return Result.Failure<None>(acceptFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
