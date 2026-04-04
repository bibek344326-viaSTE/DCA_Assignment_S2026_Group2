using Microsoft.Extensions.DependencyInjection;
using UnitTests.Fakes.CommandHandler;
using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;

namespace UnitTests.Common.Dispatcher.Decorator;

public class LoggingDispatcherDecoratorTests
{
    [Fact]
    public async Task LoggingDispatcher_WhenCommandDispatched_LogsCommandStarted()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var innerDispatcher = new CommandDispatcher(serviceProvider);
        var testLogger = new TestDispatcherLogger();
        var loggingDispatcher = new LoggingDispatcher(innerDispatcher, testLogger);

        var command = CreateEventCommand.Create().Payload!;

        // Act
        await loggingDispatcher.DispatchAsync(command);

        // Assert
        Assert.Contains("[STARTED]", testLogger.LogMessages);
    }

    [Fact]
    public async Task LoggingDispatcher_WhenCommandSucceeds_LogsCommandSucceededWithTime()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var innerDispatcher = new CommandDispatcher(serviceProvider);
        var testLogger = new TestDispatcherLogger();
        var loggingDispatcher = new LoggingDispatcher(innerDispatcher, testLogger);

        var command = CreateEventCommand.Create().Payload!;

        // Act
        await loggingDispatcher.DispatchAsync(command);

        // Assert
        Assert.NotEmpty(testLogger.SuccessfulCommands);
        Assert.True(testLogger.SuccessfulCommands[0].ExecutionTimeMs >= 0);
    }

    [Fact]
    public async Task LoggingDispatcher_WithMultipleCommands_LogsEachWithTime()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateTitleCommand>, TestUpdateTitleCommandHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var innerDispatcher = new CommandDispatcher(serviceProvider);
        var testLogger = new TestDispatcherLogger();
        var loggingDispatcher = new LoggingDispatcher(innerDispatcher, testLogger);

        var createCommand = CreateEventCommand.Create().Payload!;
        var updateCommand = UpdateTitleCommand.Create(Guid.NewGuid(), "New Title").Payload!;

        // Act
        await loggingDispatcher.DispatchAsync(createCommand);
        await loggingDispatcher.DispatchAsync(updateCommand);

        // Assert
        Assert.Equal(4, testLogger.LogMessages.Count); 
        Assert.Equal(2, testLogger.SuccessfulCommands.Count);
    }

    [Fact]
    public async Task LoggingDispatcher_PassesThroughOriginalResult()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>, TestCreateEventCommandHandler>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var innerDispatcher = new CommandDispatcher(serviceProvider);
        var testLogger = new TestDispatcherLogger();
        var loggingDispatcher = new LoggingDispatcher(innerDispatcher, testLogger);

        var command = CreateEventCommand.Create().Payload!;

        // Act
        var result = await loggingDispatcher.DispatchAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task LoggingDispatcher_DecoratorDoesNotModifyHandler()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var testHandler = new TestCreateEventCommandHandler();
        serviceCollection.AddScoped<ICommandHandler<CreateEventCommand>>(_ => testHandler);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var innerDispatcher = new CommandDispatcher(serviceProvider);
        var testLogger = new TestDispatcherLogger();
        var loggingDispatcher = new LoggingDispatcher(innerDispatcher, testLogger);

        var command = CreateEventCommand.Create().Payload!;

        // Act
        await loggingDispatcher.DispatchAsync(command);

        // Assert
        Assert.Equal(1, testHandler.CallCount);
    }
}

