using UnitTests.Features.Location;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.SetMaxGuests;

public class SetMaxGuests
{
    //ID:UC7:S1
    [Theory]
    [InlineData(5, EventStatus.Draft)]
    [InlineData(5, EventStatus.Ready)]
    [InlineData(10, EventStatus.Draft)]
    [InlineData(10, EventStatus.Ready)]
    [InlineData(25, EventStatus.Draft)]
    [InlineData(25, EventStatus.Ready)]
    [InlineData(50, EventStatus.Draft)]
    [InlineData(50, EventStatus.Ready)]
    public void SetMaxGuests_EventInDraftOrReadyStatus_MaxGuestsSet(int maxGuests, EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();

        // Act
        @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.Equal(maxGuests, @event.MaxGuests);
    }
    
    //ID:UC7:S2
    [Theory]
    [InlineData(5, EventStatus.Draft)]
    [InlineData(5, EventStatus.Ready)]
    [InlineData(10, EventStatus.Draft)]
    [InlineData(10, EventStatus.Ready)]
    [InlineData(25, EventStatus.Draft)]
    [InlineData(25, EventStatus.Ready)]
    [InlineData(50, EventStatus.Draft)]
    [InlineData(50, EventStatus.Ready)]
    public void SetMaxGuests_EventInDraftOrReadyStatus_MaxGuestsSet2(int maxGuests, EventStatus status) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(status)
            .Build();

        // Act
        @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.Equal(maxGuests, @event.MaxGuests);
    }
    
    //ID:UC7:S3
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void SetMaxGuests_EventInActiveStatus_MaxGuestsSet(int maxGuests) {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.Equal(maxGuests, @event.MaxGuests);
    }    
    
    //ID:UC7:F1
    [Fact]
    public void SetMaxGuests_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .WithMaxNumberOfGuests(25)
            .Build();
        var maxGuests = 10;

        // Act
        var result = @event.SetMaxGuests(maxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActiveAndMaxGuestsReduced.Message, result.Error.Message);
    }
    
    //ID:UC7:F2
    [Fact]
    public void SetMaxGuests_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .Build();

        // Act
        var result = @event.SetMaxGuests(25);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
    
    //ID:UC7:F3
    // ID:UC7:F3
    [Fact]
    public void SetMaxGuests_MaxGuestsLargerThanLocationMax_FailureMessageReturned()
    {
        // Arrange — create location aggregate
        var location = LocationFactory.Init()
            .WithMaxNumberOfPeople(10)
            .Build();

        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .WithLocation(location.Id)
            .Build();

        // Act
        var result = @event.SetMaxGuests(25);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(25, @event.MaxGuests);
    }

    //ID:UC7:F4
    [Fact]
    public void SetMaxGuests_MaxGuestsLessThan5_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var newMaxGuests = 4;

        // Act
        var result = @event.SetMaxGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooFewGuests(5).Message, result.Error.Message);
    }
    
    //ID:UC7:F5
    [Fact]
    public void SetMaxGuests_MaxGuestsMoreThan50_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();
        var newMaxGuests = 51;

        // Act
        var result = @event.SetMaxGuests(newMaxGuests);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooManyGuests(50).Message, result.Error.Message);
    }
}