using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViaEventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.QueryContracts;
using ViaEventAssociation.Infrastructure.EfcQueries.Handlers;
using ViaEventAssociation.Infrastructure.EfcQueries.Services;

namespace ViaEventAssociation.Infrastructure.EfcQueries.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfcQueries(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.TryAddScoped<ISystemTime, SystemTime>();

        var handlerInterfaceType = typeof(IQueryHandler<,>);
        var handlerTypes = typeof(BrowseUpcomingEventsQueryHandler).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .Select(type => new
            {
                Implementation = type,
                Interface = type.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
            })
            .Where(registration => registration.Interface is not null);

        foreach (var registration in handlerTypes)
            services.AddScoped(registration.Interface!, registration.Implementation);

        return services;
    }
}
