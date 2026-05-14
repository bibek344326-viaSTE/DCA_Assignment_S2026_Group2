using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Events;
using ViaEventAssociation.Presentation.WebAPI.Contracts.Queries;

namespace ViaEventAssociation.Presentation.WebAPI.Mapping;

public static class PresentationMappingExtensions
{
    public static IServiceCollection AddPresentationMappings(this IServiceCollection services)
    {
        services.AddScoped<IMapping<BrowseUpcomingEventsRequest, BrowseUpcomingEventsQuery>, BrowseUpcomingEventsRequestMapping>();
        services.AddScoped<IMapping<BrowseUpcomingEventsAnswer, BrowseUpcomingEventsResponse>, BrowseUpcomingEventsAnswerMapping>();
        services.AddScoped<IMapping<PersonalProfileAnswer, PersonalProfileResponse>, QueryAnswerMappings>();
        services.AddScoped<IMapping<SingleEventAnswer, SingleEventResponse>, QueryAnswerMappings>();
        services.AddScoped<IMapping<EventEditingOverviewAnswer, EventEditingOverviewResponse>, QueryAnswerMappings>();
        services.AddScoped<IMapping<EventGuestStatusOverviewAnswer, EventGuestStatusOverviewResponse>, QueryAnswerMappings>();
        return services;
    }
}
