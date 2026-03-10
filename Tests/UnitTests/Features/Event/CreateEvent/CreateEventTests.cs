using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Features.Event.CreateEvent;

public class CreateEventTests
{
    // ID: UC1.S1
    [Fact]
    public void CreateEvent_ShouldCreateAnEmptyEvent_With_Id_StatusDraft_And_MaxGuests5()
    {
        // Arrange
        EventRoot evt;
        
        // Act
        evt = EventFactory.Init().Build(); 
        
        // Assert
        Assert.Equal(EventStatus.Draft, evt.eventStatus);
        Assert.Equal(5, evt.maxGuests);
    }
    
    // ID: UC1.S2
    [Fact]
    public void CreateEvent_WithNullId_ProduceTitleWorkingTitle() {
        // Arrange
        EventRoot evt;

        // Act
        evt = EventFactory.Init().Build();

        // Assert
        Assert.Equal("Working Title", evt.eventTitle.Value);
    }
    
    // ID:UC1.S3
    [Fact]
    public void CreateEvent_WithNullId_ProduceEmptyDescription() {
        // Arrange
        EventRoot evt;

        // Act
        evt = EventFactory.Init().Build();

        // Assert

        Assert.Equal(string.Empty, evt.eventDescription.Value);
    }

    //S4

    [Fact]
    public void CreateEvent_WithNullId_ProducePrivateVisibility() {
        // Arrange
        EventRoot evt;

        // Act
        evt = EventFactory.Init().Build();

        // Assert
        Assert.False(evt.isPublic);
    }
    
}
