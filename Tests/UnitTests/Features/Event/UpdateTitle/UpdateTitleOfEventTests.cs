using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleOfEventTests
{
    //ID:UC2.S1
    [Theory]
    [InlineData("Scary Movie Night!")]
    [InlineData("333")] //min length
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time")]
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
        evt.UpdateTitle("");
        
        // Assert
        Assert.Equal("", evt.eventTitle);
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
        evt.UpdateTitle(title);
        
        // Assert
        Assert.Equal(title, evt.eventTitle);
    }
    
     // ID:UC2.F3
    [Theory]
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time")]
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time.........Enjoy Time")] // 150 characters
    public void UpdateTitle_TitleLengthMoreThan75Characters_FailureMessageReturned(string title) {
        // Arrange
        var evt = EventFactory.Init().Build();

        // Act
        evt.UpdateTitle(title);

        // Assert
        Assert.Equal(title, evt.eventTitle);
    }

    // ID:UC2.F4
    [Fact]
    public void UpdateTitle_TitleNull_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().Build();
        string title = null;

        // Act
        evt.UpdateTitle(title);

        // Assert
        Assert.Null(evt.eventTitle);
    }

    // ID:UC2.F5
    [Fact]
    public void UpdateTitle_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Active).Build();

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() => evt.UpdateTitle("Scary Movie Night!"));
    }

    // ID:UC2.F6
    [Fact]
    public void UpdateTitle_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() => evt.UpdateTitle("Scary Movie Night!"));
    }
}