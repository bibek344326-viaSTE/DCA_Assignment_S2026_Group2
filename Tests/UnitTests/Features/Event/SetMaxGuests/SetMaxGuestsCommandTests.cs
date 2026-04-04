using System;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuestsCommandTests
{
    //s1
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxGuests_WithValidMaxGuestsValues_Success(int maxGuests)
    {
        Result<SetMaxGuestsCommand> result = SetMaxGuestsCommand.Create(Guid.NewGuid(), maxGuests);
        SetMaxGuestsCommand command = result.Payload!;

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(command.Id.ToString());
        Assert.Equal(maxGuests, command.MaxGuests);
    }

    //F4
    [Fact]
    public void SetMaxGuests_WithTooFewGuests_Failure()
    {
        Result<SetMaxGuestsCommand> result = SetMaxGuestsCommand.Create(Guid.NewGuid(), 4);

        Assert.True(result.IsFailure);
        Assert.Null(result.Payload);
    }
}