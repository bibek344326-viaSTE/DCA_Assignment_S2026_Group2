using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.AppEntry;

namespace ViaEventAssociation.Core.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        var handlerInterfaceType = typeof(ICommandHandler<>);
        var assembly = Assembly.GetExecutingAssembly();
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType))
            .ToList();

        foreach (var type in handlerTypes)
        {
            var interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType);
            services.AddScoped(interfaceType, type);

            Console.WriteLine($"Registered command handler: {type.Name} with service type: {interfaceType.Name}");
        }

        return services;
    }
}

