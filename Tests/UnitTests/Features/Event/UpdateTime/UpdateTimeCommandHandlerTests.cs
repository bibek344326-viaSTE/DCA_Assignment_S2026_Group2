using UnitTests.Fakes;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Features.Event.UpdateTime;

public class UpdateTimeCommandHandlerTests
{
    [Theory]
    [InlineData("2030/08/25 19:00", "2030/08/25 23:59")]
    public async Task UpdateTime_WithValidStartAndEnd_Success(DateTime start, DateTime end)
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var cmdResult = UpdateTimeCommand.Create(@event.Id.Value, start, end);
        var command = cmdResult.Payload!;

        var handler = new UpdateTimeCommandHandler(repo, uow);

        // Act
        Result result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result is Success<None>);
        Assert.Equal(1, uow.SaveChangesCallCount);
    }

    [Theory]
    [InlineData("2030/08/26 19:00", "2030/08/25 01:00")]
    public async Task UpdateTime_WithStartAfterEnd_Failure(DateTime start, DateTime end)
    {
        // Arrange
        var repo = new FakeEventRepository();
        var uow = new FakeUnitOfWork();

        var @event = EventFactory.Init().WithStatus(EventStatus.Draft).Build();
        await repo.AddAsync(@event);

        var cmdResult = UpdateTimeCommand.Create(@event.Id.Value, start, end);

        Assert.True(cmdResult is Failure<UpdateTimeCommand>);
        Assert.Equal(0, uow.SaveChangesCallCount);
    }
}