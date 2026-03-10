using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescriptionTests
{
    //ID:UC3.S1
    [Theory]
    [InlineData("")]
    //53 characters
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximus ma")]
    //94 characters
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximus malesuada. Morbi faucibus sed neque vitae e")]
    //250 characters
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximus malesuada. Morbi faucibus sed neque vitae euismod. Vestibulum non purus vel justo ornare vulputate. In a interdum enim. Maecenas sed sodales elit, sit amet venenatis orci. Suspendisse potenti. Sed pu")]
  

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
        Assert.Equal("", evt.eventDescription);
    }
    
    //ID:UC3.S3
    
    [Theory]
    //53 characters
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximus ma")]
    //94 characters
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximus malesuada. Morbi faucibus sed neque vitae e")]
    //250 characters
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximus malesuada. Morbi faucibus sed neque vitae euismod. Vestibulum non purus vel justo ornare vulputate. In a interdum enim. Maecenas sed sodales elit, sit amet venenatis orci. Suspendisse potenti. Sed pu")]
    
    public void UpdateDescription_DescriptionLengthBetween0And250Characters_StatusReady_DescriptionUpdated_EventStatusDraft(string description)
    {
        // Arrange 
        var evt = EventFactory.Init().WithStatus(EventStatus.Ready).Build();
        
        // Act
        evt.UpdateDescription(description);
        
        // Assert
        Assert.Equal(description, evt.eventDescription);
        Assert.Equal(EventStatus.Draft, evt.eventStatus);
    }
    
    //ID:UC3.F1 
    [Theory]
    [InlineData("Nullam tempor lacus nisl, eget tempus \tquam maximus malesuada. Morbi faucibus \tsed neque vitae euismod. Vestibulum \tnon purus vel justo ornare vulputate. \tIn a interdum enim. Maecenas sed \tsodales elit, sit amet venenatis orci. \tSuspendisse potenti. Sed pulvinar \tturpis ut euismod varius. Nullam \tturpis tellus, tincidunt ut quam \tconvallis, auctor mollis nunc. Aliquam \terat volutpat.")]
    public void UpdateDescription_DescriptionLengthMoreThan250Characters_FailureMessageReturned(string description)
    {
        // Arrange
        var evt = EventFactory.Init().Build();
        
        // Act
        var result = evt.UpdateDescription(description);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.TooLongDescription(250).Message, result.Error.Message);
    }
    
    //ID:UC3.F2
    
    [Fact]
    public void UpdateDescription_EventInCancelledStatus_FailureMessageReturned()
    {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Cancelled).Build();
        
        // Act
        var result = evt.UpdateDescription("New description");
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsCanceled.Message, result.Error.Message);
    }
    
    //ID:UC3.F3
    [Fact]
    public void UpdateDescription_EventInActiveStatus_FailureMessageReturned()
    {
        // Arrange
        var evt = EventFactory.Init().WithStatus(EventStatus.Active).Build();
        
        // Act
        var result = evt.UpdateDescription("New description");
        
        // Asserts
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventStatusIsActive.Message, result.Error.Message);
    }
}