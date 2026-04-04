using System;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.UpdateTime;

public class UpdateTimeCommandTests
{
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    public void UpdateTime_WithValidStartAndEnd_Success(DateTime start, DateTime end)
    {
        Result<UpdateTimeCommand> result = UpdateTimeCommand.Create(Guid.NewGuid(), start, end);
        UpdateTimeCommand command = result.Payload!;

        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString());
    }

    [Theory]
    [InlineData("2023/08/26 19:00", "2023/08/25 01:00")]
    public void UpdateTime_WithStartAfterEnd_Failure(DateTime start, DateTime end)
    {
        Result<UpdateTimeCommand> result = UpdateTimeCommand.Create(Guid.NewGuid(), start, end);

        Assert.True(result.IsFailure);
        Assert.Null(result.Payload);
    }
}