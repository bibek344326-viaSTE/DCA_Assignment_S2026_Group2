using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

public class Guest : AggregateRoot<Email>
{
    internal Email email { get; private set; }
    internal string FirstName { get; private set; }
    internal string LastName { get; private set; }

    private Guest(Email email, string firstName, string lastName) : base(email)
    {
        this.email = email;
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    public static Result<Guest> Create(string? email, string? firstName, string? lastName)
    {
        var errors = new List<Error>();

        email = email?.Trim();
        firstName = firstName?.Trim();
        lastName = lastName?.Trim();

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

        return Result.Success(new Guest(
            ((Success<Email>)emailResult).Value,
            ((Success<string>)firstNameResult).Value,
            ((Success<string>)lastNameResult).Value
        ));
    }

    private static Result<string> ValidateAndFormatName(string? name, bool isFirstName)
    {
        if (name is null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(name))
            return Error.BlankString;

        // At this point, name is guaranteed to be non-null and non-whitespace
        // Trim and store in a non-nullable variable to help the compiler
        string trimmedName = name.Trim();

        if (trimmedName.Length < 2)
            return isFirstName ? Error.FirstNameTooShort(2) : Error.LastNameTooShort(2);

        if (trimmedName.Length > 25)
            return isFirstName ? Error.FirstNameTooLong(25) : Error.LastNameTooLong(25);

        if (!trimmedName.All(char.IsLetter))
            return isFirstName ? Error.InvalidFirstName : Error.InvalidLastName;

        var formatted = char.ToUpper(trimmedName[0]) + trimmedName.Substring(1).ToLower();
        return Result.Success(formatted);
    }

    // Behavior (simple for now)
    public Result<None> AttendEvent(Guid eventId) => Result.Success();
    public Result<None> CancelAttendance(Guid eventId) => Result.Success();
    public Result<None> RequestToJoinEvent(Guid eventId) => Result.Success();
    public Result<None> AcceptInvitation(Guid invitationId) => Result.Success();
}