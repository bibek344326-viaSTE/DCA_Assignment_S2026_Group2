using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.OperationResult;
using DomainGuest = ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate.Guest;

namespace ViaEventAssociation.Core.Application.CommandHandlers.Guest;

public class RegisterGuestCommandHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterGuestCommand>
{
    public async Task<Result> HandleAsync(RegisterGuestCommand command)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult is Failure<Email> emailFailure)
            return Result.Failure<None>(emailFailure.Errors);

        var email = emailResult.Payload!;

        var existing = await guestRepository.GetByIdAsync(email);
        if (existing.Payload is not null)
            return Result.Failure<None>(Error.EmailAlreadyRegistered);

        var guestResult = DomainGuest.Create(command.Email, command.FirstName, command.LastName, command.ProfilePictureUrl);
        if (guestResult is Failure<DomainGuest> guestFailure)
            return Result.Failure<None>(guestFailure.Errors);

        var addResult = await guestRepository.AddAsync(guestResult.Payload!);
        if (addResult is Failure<None> addFailure)
            return Result.Failure<None>(addFailure.Errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
