using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescriptionTests
{
    //ID:UC3.S1
    [Theory]
    [InlineData("")]
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a ar")]
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio")]
    [InlineData("Nam quis nulla. Integer malesuada. In in enim a arcu imperdiet malesuada. Sed vel lectus. Donec odio urna, tempus molestie, porttitor ut, iaculis quis, sem. Phasellus rhoncus. Aenean id metus id velit ullamcorper pulvinar. Vestibulum fermentum tortor")]

    public void UpdateDescription_DescriptionLengthBetween0And250Characters_StatusDraft_DescriptionUpdated(string description)
    {
        // Arrange 
        var evt = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        
        // Act
        evt.UpdateDescription(description);
        
        // Assert
        Assert.Equal(description, evt.eventDescription);
    }
    
    
    //ID:UC3.S2

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void UpdateDescription_DescriptionIsNullOrEmpty_DescriptionSetToEmptyDescription(string description)
    {
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        evt.UpdateDescription(description);
        
        // Assert
        Assert.Equal(description, evt.eventDescription);
    }
    
    //ID:UC3.S3
    
}