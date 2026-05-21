using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.Application.Extensions;
using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.Application.CommandDispatching.Commands.Event;
using ViaEventAssociation.Core.Application.CommandHandlers.Event;

namespace UnitTests.Features.Application.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCommandHandlers_ShouldRegisterCommandHandlers_WithScopedLifetime()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCommandHandlers();

        // Assert
        var serviceDescriptor = services.FirstOrDefault(sd => 
            sd.ServiceType == typeof(ICommandHandler<CreateEventCommand>) && 
            sd.ImplementationType == typeof(CreateEventCommandHandler));

        Assert.NotNull(serviceDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, serviceDescriptor.Lifetime);
    }
}