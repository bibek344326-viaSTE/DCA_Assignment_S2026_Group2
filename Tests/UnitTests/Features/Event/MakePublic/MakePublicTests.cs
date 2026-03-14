using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakePublic;

public class MakePublicTests
{
    //ID:UC5.S1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void MakePublic_EventInDraftStatus_EventIsPublic_StatusIsUnchanged(EventStatus eventStatus)
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(eventStatus)
            .Build();

        // Act
        @event.MakePublic();

        // Assert
        Assert.Equal(EventVisibility.Public, @event.Visibility);
        Assert.Equal(eventStatus, @event.Status);
    }
    
    //ID:UC5.F1
    [Fact]
    public void MakePublic_EventInCancelledStatus_FailureMessageReturned()
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.MakePublic();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}