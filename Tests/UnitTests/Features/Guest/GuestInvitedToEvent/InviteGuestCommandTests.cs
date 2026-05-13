using ViaEventAssociation.Core.AppEntry.Commands.Guest;

namespace UnitTests.Features.Guests;

public class InviteGuestCommandTests
{
    [Fact]
    public void Create_ValidInput_ReturnsCommand()
    {
        var result = InviteGuestCommand.Create(Guid.NewGuid(), "abc@via.dk");

        Assert.True(result.IsSuccess);
        Assert.Equal("abc@via.dk", result.Payload!.GuestEmail.Value);
    }

    [Fact]
    public void Create_InvalidEmail_ReturnsFailure()
    {
        var result = InviteGuestCommand.Create(Guid.NewGuid(), "not-an-email");

        Assert.True(result.IsFailure);
    }
}
