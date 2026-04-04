using System;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;
using Xunit;

namespace UnitTests.Features.Event.MakePublic;

public class MakePublicCommandTests
{
    [Fact]
    public void MakePublic_WithValidId_Success()
    {
        Result<MakeEventPublicCommand> result = MakeEventPublicCommand.Create(Guid.NewGuid());
        MakeEventPublicCommand command = result.Payload!;

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(command.Id.ToString());
    }

    [Fact]
    public void MakePublic_WithInvalidId_Failure()
    {
        Result<EventId> idResult = EventId.Create("not-a-guid");
        Assert.True(idResult.IsFailure);
    }
}