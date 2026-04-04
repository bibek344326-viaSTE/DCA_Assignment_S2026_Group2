using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Guests.RegisterGuests;

public class RegisterGuestTests
{
    // S1
    [Fact]
    public void RegisterGuest_AllValidData_GuestIsCreated()
    {
        var result = Guest.Create("abc@via.dk", "John", "Doe");

        Assert.True(result.IsSuccess);
        var guest = ((Success<Guest>)result).Value;

        Assert.Equal("abc@via.dk", guest.email.Value);
        Assert.Equal("John", guest.FirstName);
        Assert.Equal("Doe", guest.LastName);
    }

    // S2
    [Fact]
    public void RegisterGuest_EmailUpperCase_IsNormalizedToLowerCase()
    {
        var result = Guest.Create("ABC@VIA.DK", "John", "Doe");

        Assert.True(result.IsSuccess);
        var guest = ((Success<Guest>)result).Value;

        Assert.Equal("abc@via.dk", guest.email.Value);
    }

    // S3
    [Fact]
    public void RegisterGuest_NamesMixedCase_AreCapitalized()
    {
        var result = Guest.Create("abc@via.dk", "jOhN", "dOe");

        Assert.True(result.IsSuccess);
        var guest = ((Success<Guest>)result).Value;

        Assert.Equal("John", guest.FirstName);
        Assert.Equal("Doe", guest.LastName);
    }

    // F1 - Invalid email
    [Theory]
    [InlineData("abc@gmail.com", "EMAIL_MUST_BE_VIA")]
    [InlineData("ab@via.dk", "INVALID_VIA_EMAIL_IDENTIFIER")]
    [InlineData("abc123@via.dk", "INVALID_VIA_EMAIL_IDENTIFIER")]
    [InlineData("", "BLANK_STRING")]
    public void RegisterGuest_InvalidEmail_ShouldFail(string email, string expectedError)
    {
        var result = Guest.Create(email, "John", "Doe");

        Assert.True(result.IsFailure);
        Assert.Contains(((Failure<Guest>)result).Errors,
            e => e.Code == expectedError);
    }

    // F2 - Invalid first name
    [Theory]
    [InlineData("J", "FIRST_NAME_TOO_SHORT")]
    [InlineData("John123", "INVALID_FIRST_NAME")]
    [InlineData("", "BLANK_STRING")]
    public void RegisterGuest_InvalidFirstName_ShouldFail(string name, string expectedError)
    {
        var result = Guest.Create("abc@via.dk", name, "Doe");

        Assert.True(result.IsFailure);
        Assert.Contains(((Failure<Guest>)result).Errors,
            e => e.Code == expectedError);
    }

    // F3 - Invalid last name
    [Theory]
    [InlineData("D", "LAST_NAME_TOO_SHORT")]
    [InlineData("Doe123", "INVALID_LAST_NAME")]
    [InlineData("", "BLANK_STRING")]
    public void RegisterGuest_InvalidLastName_ShouldFail(string name, string expectedError)
    {
        var result = Guest.Create("abc@via.dk", "John", name);

        Assert.True(result.IsFailure);
        Assert.Contains(((Failure<Guest>)result).Errors,
            e => e.Code == expectedError);
    }

    // Edge case
    [Fact]
    public void RegisterGuest_InputWithWhitespace_ShouldTrimAndSucceed()
    {
        var result = Guest.Create("  abc@via.dk  ", "  John  ", "  Doe  ");

        Assert.True(result.IsSuccess);
        var guest = ((Success<Guest>)result).Value;

        Assert.Equal("abc@via.dk", guest.email.Value);
        Assert.Equal("John", guest.FirstName);
        Assert.Equal("Doe", guest.LastName);
    }

    // Multiple errors
    [Fact]
    public void RegisterGuest_MultipleInvalidFields_ShouldReturnAllErrors()
    {
        var result = Guest.Create("invalid@gmail.com", "J", "D");

        Assert.True(result.IsFailure);
        var errors = ((Failure<Guest>)result).Errors;

        Assert.True(errors.Count() >= 3);
        Assert.Contains(errors, e => e.Code == "EMAIL_MUST_BE_VIA");
        Assert.Contains(errors, e => e.Code == "FIRST_NAME_TOO_SHORT");
        Assert.Contains(errors, e => e.Code == "LAST_NAME_TOO_SHORT");
    }
}