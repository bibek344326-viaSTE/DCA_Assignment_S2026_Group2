using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.SetReadyEvent;

public class ReadiesEventTests
{
    //UC8.S1
    [Fact]
    public void ReadiesEvent_EventInDraftStatus_DataSetWithValidValues_EventIsReady()
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithPublicVisibility()
            .WithMaxNumberOfGuests(10)
            .Build();

        // Act
        var result = @event.Ready();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Ready, @event.Status);
    }

    //UC8.F1
    [Theory]
    [InlineData(false, true, true, true, 10, "EVENT_TITLE_DEFAULT")]
    [InlineData(true, false, true, true, 10, "EVENT_DESCRIPTION_MISSING")]
    [InlineData(true, true, false, true, 10, "EVENT_DATE_TIME_MISSING")]
    [InlineData(true, true, true, false, 10, "EVENT_VISIBILITY_MISSING")]
    public void SetReadyEvent_EventIsDraft_MissingOrInvalidData_FailureMessageReturned(
        bool setTitle,
        bool setDescription,
        bool setTime,
        bool setVisibility,
        int maxGuests,
        string expectedErrorCode)
    {
        // Arrange
        var builder = EventFactory.Init();

        if (setTitle)
            builder.WithValidTitle();

        if (setDescription)
            builder.WithValidDescription();

        if (setTime)
            builder.WithValidTimeInFuture();

        if (setVisibility)
            builder.WithPublicVisibility();

        builder.WithMaxNumberOfGuests(maxGuests);

        var @event = builder.Build();

        // Act
        var result = @event.Ready();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(expectedErrorCode, result.Error.Code);
    }
    
    //UC8.F2
    
    [Fact]
    public void ReadiesEvent_EventIsCancelled_FailureMessageReturned()
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Cancelled)
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithPublicVisibility()
            .WithMaxNumberOfGuests(10)
            .Build();

        // Act
        var result = @event.Ready();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled.Code, result.Error.Code);
    }
    
    //UC8.F3
    [Fact]
    public void SetReadyEvent_EventInPast_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithPublicVisibility()
            .WithMaxNumberOfGuests(10)
            .Build();

        var start = DateTime.Now.AddDays(-1).Date.AddHours(10);
        var end = start.AddHours(2);

        var startProp = typeof(EventRoot).GetProperty("eventStartDateTime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var endProp = typeof(EventRoot).GetProperty("eventEndDateTime", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        startProp?.SetValue(@event, start);
        endProp?.SetValue(@event, end);

        // Act
        var result = @event.Ready();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStartTimeInThePast.Code, result.Error.Code);
    }
    
    //UC8.F4
    
    [Fact]
    public void SetReadyEvent_EventTitleIsDefault_FailureMessageReturned() {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Draft)
            .Build();

        // Act
        var result = @event.Ready();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventTitleIsDefault.Message, result.Error.Message);
    }
}