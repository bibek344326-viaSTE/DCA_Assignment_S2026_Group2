using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

public class Guest : AggregateRoot<Email>
{
    internal Email email { get; private set; }
    internal string firstName { get; private set; }
    internal string lastName { get; private set; }

    private Guest(Email guestEmail) : base(guestEmail)
    {
        email = guestEmail;
        firstName = string.Empty;
        lastName = string.Empty;
    }

    public static Guest Create(string email, string firstName, string lastName)
    {
        var errors = new List<Error>();

        var emailResult = Email.Create(email);
        if (emailResult is Failure<Email> emailFailure)
            errors.AddRange(emailFailure.Errors);

        var firstNameResult = ValidateAndFormatName(firstName, true);
        if (firstNameResult is Failure<string> firstNameFailure)
            errors.AddRange(firstNameFailure.Errors);

        var lastNameResult = ValidateAndFormatName(lastName, false);
        if (lastNameResult is Failure<string> lastNameFailure)
            errors.AddRange(lastNameFailure.Errors);

        if (errors.Any())
            return Result.Failure<Guest>(errors);

        var validEmail = ((Success<Email>)emailResult).Value;
        var validFirstName = ((Success<string>)firstNameResult).Value;
        var validLastName = ((Success<string>)lastNameResult).Value;

        return Result.Success(new Guest(validEmail, validFirstName, validLastName));
    }

    private static Result<string> ValidateAndFormatName(string? name, bool isFirstName)
    {
        if (name is null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(name))
            return Error.BlankString;

        name = name.Trim();

        if (name.Length < 2)
            return isFirstName ? Error.FirstNameTooShort(2) : Error.LastNameTooShort(2);

        if (name.Length > 25)
            return isFirstName ? Error.FirstNameTooLong(25) : Error.LastNameTooLong(25);

        if (!name.All(char.IsLetter))
            return isFirstName ? Error.InvalidFirstName : Error.InvalidLastName;

        var formatted = char.ToUpper(name[0]) + name.Substring(1).ToLower();

        return Result.Success(formatted);
    }

    public Result<None> AttendEvent(Guid eventId)
    {
        return Result.Success();
    }

    public Result<None> CancelAttendance(Guid eventId)
    {
        return Result.Success();
    }

    public Result<None> RequestToJoinEvent(Guid eventId)
    {
        return Result.Success();
    }

    public Result<None> AcceptInvitation(Guid invitationId)
    {
        return Result.Success();
    }
}