using System;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescriptionCommandTests
{
    [Fact]
    public void UpdateDescription_WithValidIdAndDescription_Success()
    {
        Result<UpdateDescriptionCommand> result = UpdateDescriptionCommand.Create(Guid.NewGuid(), Guid.NewGuid().ToString());
        UpdateDescriptionCommand command = result.Payload!;

        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString());
    }

    [Fact]
    public void UpdateDescription_WithDescriptionTooLong_Failure()
    {
        string tooLongDescription = new string('a', 251);

        Result<UpdateDescriptionCommand> result = UpdateDescriptionCommand.Create(Guid.NewGuid(), tooLongDescription);

        Assert.True(result.IsFailure);
        Assert.Null(result.Payload);
    }
}
