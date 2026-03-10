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
        var convertedTitle = EventTitle.Create(title);
        
        // Act
        evt.UpdateTitle(convertedTitle);
        
        // Assert
        Assert.Equal(title, evt.eventTitle.Value);
    }
    
    //ID.UC2.S2
    [Fact]
    public void UpdateTitleOfEvent_TitleLengthBetween3And75Characters_EventInReadyStatus_TitleUpdated_EventInDraftStatus()
    {
       // Arrange 
       var evt = EventFactory.Init().WithStatus(EventStatus.Ready).Build();
       var convertedTitle = EventTitle.Create("Graduation Gala");
       
       // Act
       evt.UpdateTitle(convertedTitle);
       
         // Assert
        Assert.Equal("Graduation Gala", evt.eventTitle.Value);
        Assert.Equal(EventStatus.Draft, evt.eventStatus);

    }

    //ID:UC2.F1
    [Fact]
    public void UpdateTitle_TitleLength0Characters_FailureMessageReturned()
    {
        // Arrange
        var evt = EventFactory.Init().Build();

        // Act
        var result = EventTitle.Create("");
        
        // Assert
        Assert.Equal("", result.Value);
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
        var result = EventTitle.Create(title);
        
        // Assert
        Assert.Equal(title, result.Value);
    }
    
     // ID:UC2.F3
    [Theory]
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time")]
    [InlineData("Fun Night With Friends Watching Movies Eating Snacks And Enjoying Time.........Enjoy Time")] // 150 characters
    public void UpdateTitle_TitleLengthMoreThan75Characters_FailureMessageReturned(string title) {
        // Arrange
        var evt = EventFactory.Init().Build();

        // Act
        var result = EventTitle.Create(title);

        // Assert
        Assert.Equal(title, result.Value);
    }

    // ID:UC2.F4
    [Fact]
    public void UpdateTitle_TitleNull_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().Build();
        string title = null;

        // Act
        var result = EventTitle.Create(title);

        // Assert
        Assert.Null(result.Value);
    }

    // ID:UC2.F5
    [Fact]
    public void UpdateTitle_EventInActiveStatus_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Active).Build();
        var convertedTitle = EventTitle.Create("Scary Movie Night!");

        // Act
        evt.UpdateTitle(convertedTitle);
        
        // Assert
        Assert.Equal("Scary Movie Night!", evt.eventTitle.Value);
    }

    // ID:UC2.F6
    [Fact]
    public void UpdateTitle_EventInCancelledStatus_FailureMessageReturned() {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();
        var convertedTitle = EventTitle.Create("Scary Movie Night!");

        // Act
        evt.UpdateTitle(convertedTitle);

        // Assert
        Assert.Equal("Scary Movie Night!", evt.eventTitle.Value);
    }
}