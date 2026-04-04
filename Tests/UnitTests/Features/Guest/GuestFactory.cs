using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

public class GuestFactory
{
    private string? _email = "abc@via.dk";
    private string? _firstName = "John";
    private string? _lastName = "Doe";

    public static GuestFactory Init() => new();

    public GuestFactory WithValidEmail()
    {
        _email = "abc@via.dk";
        return this;
    }

    public GuestFactory WithInvalidEmail(string email)
    {
        _email = email;
        return this;
    }

    public GuestFactory WithValidFirstName()
    {
        _firstName = "John";
        return this;
    }

    public GuestFactory WithInvalidFirstName(string name)
    {
        _firstName = name;
        return this;
    }

    public GuestFactory WithValidLastName()
    {
        _lastName = "Doe";
        return this;
    }

    public GuestFactory WithInvalidLastName(string name)
    {
        _lastName = name;
        return this;
    }

    public Result<Guest> Build()
    {
        return Guest.Create(_email, _firstName, _lastName);
    }
}