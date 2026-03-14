using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateTests
{
    //ID:UC6.S1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    public void MakePrivate_EventInDraftOrReadyStatus_EventIsPrivate_StatusIsUnchanged(EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();

        // Act
        @event.MakePrivate();

        // Assert
        Assert.False(@event.IsPublic);
        Assert.Equal(status, @event.Status);
    }
    
    //ID:UC6.S2
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    public void MakePrivate_EventInDraftOrReadyStatus_EventIsPublic_EventIsPrivate_StatusIsUnchanged(EventStatus status)
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();
        
        // Act
        @event.MakePrivate();
        
        // Assert
        Assert.False(@event.IsPublic);
        Assert.Equal(status, @event.Status);
    }
        
    // ID:UC6.F1
    [Fact]
    public void MakePrivate_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        var result = @event.MakePrivate();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }
    
    // ID:UC6.F2
    [Fact]
    public void MakePrivate_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.MakePrivate();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}
    