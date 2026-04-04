using System;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleCommandTests
{
    [Fact]
    public void UpdateTitle_WithValidIdAndTitle_Success()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var newTitle = "New event title";

        // Act
        Result<UpdateTitleCommand> result = UpdateTitleCommand.Create(eventId, newTitle);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);

        var command = result.Payload!;
        Assert.Equal(eventId, command.EventId.Value);
        Assert.Equal(newTitle, command.title);
    }

    [Fact]
    public void UpdateTitle_WithEmptyTitle_Failure()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var invalidTitle = "";

        // Act
        Result<UpdateTitleCommand> result = UpdateTitleCommand.Create(eventId, invalidTitle);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Null(result.Payload);
    }
}
