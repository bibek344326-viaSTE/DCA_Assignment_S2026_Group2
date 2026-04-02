using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string? email)
    {
        if (email is null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(email))
            return Error.BlankString;

        email = email.Trim().ToLower();

        if (email.Length > 254)
            return Error.TooLongEmail(254);

        if (!IsValidEmail(email))
            return Error.InvalidEmail;

        if (!email.EndsWith("@via.dk"))
            return Error.EmailMustBeVia;

        var localPart = email.Split('@')[0];

        var validLetters = Regex.IsMatch(localPart, @"^[a-zA-Z]{3,4}$");
        var validDigits = Regex.IsMatch(localPart, @"^[0-9]{6}$");

        if (!validLetters && !validDigits)
            return Error.InvalidViaEmailIdentifier;

        return Result.Success(new Email(email));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}