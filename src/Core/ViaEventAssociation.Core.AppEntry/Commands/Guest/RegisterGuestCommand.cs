using ViaEventAssociation.Core.Application.CommandDispatching.Commands;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Guest;

public class RegisterGuestCommand : Command<string>
{
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string? ProfilePictureUrl { get; }

    private RegisterGuestCommand(string email, string firstName, string lastName, string? profilePictureUrl) : base(email)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        ProfilePictureUrl = profilePictureUrl;
    }

    public static Result<RegisterGuestCommand> Create(string? email, string? firstName, string? lastName, string? profilePictureUrl)
    {
        if (email is null)
            return Error.NullString;

        if (firstName is null)
            return Error.NullString;

        if (lastName is null)
            return Error.NullString;

        return Result.Success(new RegisterGuestCommand(email, firstName, lastName, profilePictureUrl));
    }
}
