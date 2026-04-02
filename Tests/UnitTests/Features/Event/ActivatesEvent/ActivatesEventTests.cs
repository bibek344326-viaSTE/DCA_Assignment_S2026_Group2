using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.ActivatesEvent;

public class ActivatesEventTests
{
    // UC9.S1 
    [Fact]
    public void ActivateEvent_EventInDraftStatus_AllValidDataSet_EventIsActivated()
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
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    // UC9.S2 
    [Fact]
    public void ActivateEvent_EventInReadyStatus_EventIsActivated()
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithValidTitle()
            .WithValidDescription()
            .WithValidTimeInFuture()
            .WithPublicVisibility()
            .WithMaxNumberOfGuests(10)
            .Build();

        // First make it ready
        @event.Ready();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    // UC9.S3 
    [Fact]
    public void ActivateEvent_EventInActiveStatus_EventRemainsActive()
    {
        // Arrange
        var @event = EventFactory.Init()
            .WithStatus(EventStatus.Active)
            .Build();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EventStatus.Active, @event.Status);
    }

    // UC9.F1 
    [Theory]
    [InlineData(false, true, true, true, 10, false, "EVENT_TITLE_DEFAULT")]
    [InlineData(true, false, true, true, 10, false, "EVENT_DESCRIPTION_MISSING")]
    [InlineData(true, true, false, true, 10, false, "EVENT_DATE_TIME_MISSING")]
    [InlineData(true, true, true, false, 10, false, "EVENT_VISIBILITY_MISSING")]
    [InlineData(true, true, true, true, 3, true, "INVALID_MAX_GUESTS_RANGE")]
    public void ActivateEvent_EventInDraftStatus_MissingOrInvalidData_FailureMessageReturned(
        bool setTitle,
        bool setDescription,
        bool setTime,
        bool setVisibility,
        int maxGuests,
        bool useInvalidGuestCount,
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

        if (useInvalidGuestCount)
            builder.WithMaxNumberOfGuestsInvalid(maxGuests);
        else
            builder.WithMaxNumberOfGuests(maxGuests);

        var @event = builder.Build();

        // Act
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(expectedErrorCode, result.Error.Code);
    }

    // UC9.F2 
    [Fact]
    public void ActivateEvent_EventIsCancelled_FailureMessageReturned()
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
        var result = @event.Activate();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsCanceled.Code, result.Error.Code);
    }

   }