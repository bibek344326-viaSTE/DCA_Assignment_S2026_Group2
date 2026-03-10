using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleOfEventTests
{
    //ID:UC2.S1
    [Theory]
    [InlineData("Scary Movie Night!")]
    [InlineData("333")] //min length
    [InlineData("Fun Night With Friends Watching Movies Eating Snack And Enjoying Time Enjoy")]
    public void UpdateTitleOfEvent_TitleLengthBetween3And75Characters_EventInDraftStatus_TitleUpdated(string title)
    {
        // Arrange 
        var evt = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        
        // Act
        evt.UpdateTitle(title);
        
        // Assert
        Assert.Equal(title, evt.eventTitle);
    }
    
    //ID.UC2.S2
    [Fact]
    public void UpdateTitleOfEvent_TitleLengthBetween3And75Characters_EventInReadyStatus_TitleUpdated_EventInDraftStatus()
    {
       // Arrange 
       var evt = EventFactory.Init().WithStatus(EventStatus.Ready).Build();
       
       // Act
       evt.UpdateTitle("Graduation Gala");
       
         // Assert
        Assert.Equal("Graduation Gala", evt.eventTitle);
        Assert.Equal(EventStatus.Draft, evt.eventStatus);

    }

    //ID:UC2.F1
    [Fact]
    public void UpdateTitle_TitleLength0Characters_FailureMessageReturned()
    {
        // Arrange
        var evt = EventFactory.Init().Build();

        // Act
        var result = evt.UpdateTitle("");
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.BlankString.Message, result.Error.Message);
    }
    
        //ID:UC2.F2
    [Theory]
    [InlineData("XY")]
    [InlineData("a")]
    public void UpdateTitle_TitleIsLessThan3Characters_FailureMessageReturned(string title)
    {
        // Arrange
        var evt = EventFactory.Init().Build();

        // Act
        var result = evt.UpdateTitle(title);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooShortTitle(3).Message, result.Error.Message);
    }
    
     // ID:UC2.F3
    [Theory]
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time With Extra Fun")]
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time.........Enjoy Time")] // 150 characters
    public void UpdateTitle_TitleLengthMoreThan75Characters_FailureMessageReturned(string title) {
        // Arrange
        var evt = EventFactory.Init().Build();

        // Act
        var result = evt.UpdateTitle(title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongTitle(75).Message, result.Error.Message);
    }

    // ID:UC2.F4
    [Fact]
    public void UpdateTitle_TitleNull_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().Build();
        string title = null;

        // Act
        var result = evt.UpdateTitle(title);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.NullString.Message, result.Error.Message);
    }

    // ID:UC2.F5
    [Fact]
    public void UpdateTitle_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Active).Build();

       // Act
        var result = evt.UpdateTitle("Scary Movie Night!");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }

    // ID:UC2.F6
    [Fact]
    public void UpdateTitle_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();

        // Act
        var result = evt.UpdateTitle("Scary Movie Night!");
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
}