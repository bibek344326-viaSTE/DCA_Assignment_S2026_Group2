using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

public class Guest : AggregateRoot<Email>
{
    internal Email email { get; private set; }
    internal string FirstName { get; private set; }
    internal string LastName { get; private set; }
    internal Uri ProfilePictureUrl { get; private set; }

    private Guest(Email email, string firstName, string lastName, Uri profilePictureUrl) : base(email)
    {
        this.email = email;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.ProfilePictureUrl = profilePictureUrl;
    }

    public static Result<Guest> Create(string? email, string? firstName, string? lastName)
        => Create(email, firstName, lastName, "https://example.com/default-profile-picture.jpg");

    public static Result<Guest> Create(string? email, string? firstName, string? lastName, string? profilePictureUrl)
    {
        var errors = new List<Error>();

        email = email?.Trim();
        firstName = firstName?.Trim();
        lastName = lastName?.Trim();
        profilePictureUrl = profilePictureUrl?.Trim();

        var emailResult = Email.Create(email);
        if (emailResult is Failure<Email> emailFailure)
            errors.AddRange(emailFailure.Errors);

        var firstNameResult = ValidateAndFormatName(firstName, true);
        if (firstNameResult is Failure<string> firstNameFailure)
            errors.AddRange(firstNameFailure.Errors);

        var lastNameResult = ValidateAndFormatName(lastName, false);
        if (lastNameResult is Failure<string> lastNameFailure)
            errors.AddRange(lastNameFailure.Errors);

        var profilePictureUrlResult = ValidateProfilePictureUrl(profilePictureUrl);
        if (profilePictureUrlResult is Failure<Uri> profilePictureUrlFailure)
            errors.AddRange(profilePictureUrlFailure.Errors);

        if (errors.Any())
            return Result.Failure<Guest>(errors);

        return Result.Success(new Guest(
            ((Success<Email>)emailResult).Value,
            ((Success<string>)firstNameResult).Value,
            ((Success<string>)lastNameResult).Value,
            ((Success<Uri>)profilePictureUrlResult).Value
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

    private static Result<Uri> ValidateProfilePictureUrl(string? profilePictureUrl)
    {
        if (string.IsNullOrWhiteSpace(profilePictureUrl))
            return Error.InvalidProfilePictureUrl;

        return Uri.TryCreate(profilePictureUrl, UriKind.Absolute, out var uri)
            ? Result.Success(uri)
            : Error.InvalidProfilePictureUrl;
    }

    // Behavior (simple for now)
    public Result<None> AttendEvent(EventId eventId) => Result.Success();
    public Result<None> CancelAttendance(EventId eventId) => Result.Success();
    public Result<None> RequestToJoinEvent(EventId eventId) => Result.Success();
    public Result<None> AcceptInvitation(InvitationId invitationId) => Result.Success();
}