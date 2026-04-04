using System;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateCommandTests
{
    [Fact]
    public void MakePrivate_WithValidId_Success()
    {
        Result<MakeEventPrivateCommand> result = MakeEventPrivateCommand.Create(Guid.NewGuid());
        MakeEventPrivateCommand command = result.Payload!;

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(command.Id.ToString());
    }

    [Fact]
    public void MakePrivate_WithInvalidId_Failure()
    {
        Result<EventId> idResult = EventId.Create("not-a-guid");

        Assert.True(idResult.IsFailure);
    }
}