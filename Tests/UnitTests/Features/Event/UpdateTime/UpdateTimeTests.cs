using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.UpdateTime;

public class UpdateTimeTests
{
    //ID:UC4.S1
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTime_EventInDraftStatus_StartTimeIsBeforeEndTime_DatesAreSame_Duration1HourOrLonger_StartTimeIsAfter8AM_EndBefore2359_TimesUpdated(
            DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(startTime, evt.EventStartDateTime);
        Assert.Equal(endTime, evt.EventEndDateTime);
    }
    
    //ID:UC4.S2

    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/26 01:00")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    public  void UpdateTime_EventInDraftStatus_StartDateBeforeEndDate_DurationIs1HourOrLonger_StartTimeAfter8_EndTimeBefore1_TimesUpdated(DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(startTime, evt.EventStartDateTime);
        Assert.Equal(endTime, evt.EventEndDateTime);
        Assert.Equal(EventStatus.Draft, evt.EventStatus);
    }
    
    //ID:UC4.S3
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTime_EventStatusReady_StartTimeIsBeforeEndTime_DatesAreSame_Duration1HourOrLonger_StartTimeIsAfter8AM_EndBefore2359_TimesUpdated_StatusDraft(DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Ready).Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);

        Assert.Equal(startTime, evt.EventStartDateTime);
        Assert.Equal(endTime, evt.EventEndDateTime);
        Assert.Equal(EventStatus.Draft, evt.EventStatus);
    }
    
    // ID:UC4.s4
    
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTimeInterval_StartTimeIsInTheFuture_TimesUpdated(DateTime startTime, DateTime endTime) {
        
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, evt.EventStartDateTime);
        Assert.Equal(endTime, evt.EventEndDateTime);
    }
    
    //ID:UC4.S5
    
    [Theory]
    [InlineData("2023/08/25 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/25 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/25 08:00", "2023/08/25 12:15")]
    [InlineData("2023/08/25 10:00", "2023/08/25 20:00")]
    [InlineData("2023/08/25 13:00", "2023/08/25 23:00")]
    public void UpdateTimeInterval_DurationFromStartToFinishIs10HoursOrLess_TimesUpdated(DateTime startTime, DateTime endTime) {
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, evt.EventStartDateTime);
        Assert.Equal(endTime, evt.EventEndDateTime);
    }

    //ID:UC4.F1

    [Theory]
    [InlineData("2023/08/26 19:00", "2023/08/25 01:00")]
    [InlineData("2023/08/26 19:00", "2023/08/25 23:59")]
    [InlineData("2023/08/27 12:00", "2023/08/25 16:30")]
    [InlineData("2023/08/01 08:00", "2023/07/31 12:15")]
    public void UpdateTime_StartDateIsAfterEndDate_FailureMessageReturned(DateTime startTime, DateTime endTime)
    {
        // Arrange 
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidDateTimeRange.Message, result.Error.Message);
    }
    
    // ID:UC4:F2
    [Theory]
    [InlineData("2023/08/26 19:00", "2023/08/26 14:00")]
    [InlineData("2023/08/26 16:00", "2023/08/26 00:00")]
    [InlineData("2023/08/26 19:00", "2023/08/26 18:59")]
    [InlineData("2023/08/26 12:00", "2023/08/26 10:10")]
    [InlineData("2023/08/26 08:00", "2023/08/26 00:30")]
    public void UpdateTime_StartDateIsSameAsEndDate_StartTimeIsAfterEndTime_FailureMessageReturned(DateTime startTime, DateTime endTime){
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.InvalidDateTimeRange.Message, result.Error.Message);
    }
    
    //ID:UC4:F3

    [Theory]
    [InlineData("2023/08/26 14:00", "2023/08/26 14:50")]
    [InlineData("2023/08/26 18:00", "2023/08/26 18:59")]
    [InlineData("2023/08/26 12:00", "2023/08/26 12:30")]
    [InlineData("2023/08/26 08:00", "2023/08/26 08:00")]
    public void UpdateTime_StartDateSameAsEndDate_StartTimeLessThan1HourBeforeEndTime_FailureMessageReturned(DateTime startTime, DateTime endTime)
    {
        // Arrange 
        var evt =  EventFactory.Init().Build();
        
        //Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains((result as Failure<None>)!.Errors, e => e == Error.DurationTooShort);
    }
    
    //ID:UC4.F4
    [Theory]
    [InlineData("2023/08/25 23:30", "2023/08/26 00:15")]
    [InlineData("2023/08/30 23:01", "2023/08/31 00:00")]
    [InlineData("2023/08/30 23:59", "2023/08/31 00:01")]
    public void UpdateTime_StartDateIsBeforeEndDate_StartTimeIsLessThan1HourBeforeEndTime_FailureMessageReturned(
        DateTime startTime, DateTime endTime)
    {
        // Arrange 
        var evt = EventFactory.Init().Build();
        
        //Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains((result as Failure<None>)!.Errors, e => e == Error.DurationTooShort);
    }
    
    //ID:UC4.F5
    [Theory]
    [InlineData("2023/08/25 07:50", "2023/08/25 14:00")]
    [InlineData("2023/08/25 07:59", "2023/08/25 15:00")]
    [InlineData("2023/08/25 01:01", "2023/08/25 08:30")]
    [InlineData("2023/08/25 05:59", "2023/08/25 07:59")]
    [InlineData("2023/08/25 00:59", "2023/08/25 07:59")]
    public void UpdateTime_StartTimeBefore8_FailureMessageReturned(DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(
            (result as Failure<None>)!.Errors,
            e => e == Error.InvalidStartDateTime
        );
    }
    
    //ID:UC4.F6
    [Theory]
    [InlineData("2023/08/24 23:50", "2023/08/25 01:01")]
    [InlineData("2023/08/24 22:00", "2023/08/25 07:59")]
    [InlineData("2023/08/30 23:00", "2023/08/31 02:30")]
    [InlineData("2023/08/24 23:50", "2023/08/25 01:01")]
    public void UpdateTime_StartTimeBefore1_EndTimeAfter1_FailureMessageReturned( DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(
            (result as Failure<None>)!.Errors,
            e => e == Error.InvalidEndDateTime
        );
        
    }
    
    //ID:UC4.F7
    [Fact]
    public void UpdateTime_EventInActiveStatus_FailureMessageReturned()
    {
        //Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Active).Build();
        var validStartTime = DateTime.Parse("2023/08/25 19:00");
        var validEndTime = DateTime.Parse("2023/08/25 23:59");
        
        //Act
        var result = evt.UpdateDateTime(validStartTime, validEndTime);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }
    
    //ID:UC4.F8
    [Fact]
    public void UpdateTime_EventInCancelledStatus_FailureMessageReturned()
    {
        //Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();
        var validStartTime = DateTime.Parse("2023/08/25 19:00");
        var validEndTime = DateTime.Parse("2023/08/25 23:59");
        
        //Act
        var result = evt.UpdateDateTime(validStartTime, validEndTime);
        
        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
    
    //ID:UC4.F9
    [Theory]
    [InlineData("2023/08/30 08:00", "2023/08/30 18:01")]
    [InlineData("2023/08/30 14:59", "2023/08/31 01:00")]
    [InlineData("2023/08/30 14:00", "2023/08/31 00:01")]
    [InlineData("2023/08/30 14:00", "2023/08/31 18:30")]
    public void UpdateTime_DurationIsLongerThan10Hours_FailureMessageReturned(DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(
            (result as Failure<None>)!.Errors,
            e => e == Error.DurationTooLong
        );
    }
    
    //ID:UC4.F10
    [Fact]
    public void UpdateTime_StartTimeIsInPast_FailureMessageReturned()
    {
        // Arrange
        var evt = EventFactory.Init().Build();
        var validStartTime = DateTime.Parse("2023/08/25 19:00");
        var validEndTime = DateTime.Parse("2023/08/25 23:59");

        // Act
        var result = evt.UpdateDateTime(validStartTime, validEndTime);
        
        // Assert
         Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStartTimeInThePast.Message, result.Error.Message);
    }
    
    //ID:UC4.F11
    [Theory]
    [InlineData("2023/08/31 00:30", "2023/08/31 08:30")]
    [InlineData("2023/08/30 23:59", "2023/08/31 08:01")]
    [InlineData("2023/08/31 01:00", "2023/08/31 08:00")]
    public void UpdateTime_StartTimeBefore01EndTimeAfter08_FailureMessageReturned(
        DateTime startTime, DateTime endTime)
    {
        // Arrange
        var evt = EventFactory.Init().Build();
    
        // Act
        var result = evt.UpdateDateTime(startTime, endTime);
    
        // Assert
        Assert.True(result.IsFailure);

        var failure = result as Failure<None>;
        Assert.NotNull(failure);

        var errorMessages = failure.Errors.Select(e => e.Message).ToList();

        Assert.True(
            errorMessages.Contains(Error.InvalidStartDateTime.Message) ||
            errorMessages.Contains(Error.InvalidEndDateTime.Message),
            "Expected invalid start or end time error"
        );
    }
}