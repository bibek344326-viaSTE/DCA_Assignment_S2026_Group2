using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.QueryContracts;

namespace IntegrationTests.Presentation;

internal sealed class PresentationWebApplicationFactory(
    IDispatcher? dispatcher = null,
    IQueryDispatcher? queryDispatcher = null)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            if (dispatcher is not null)
            {
                services.RemoveAll<IDispatcher>();
                services.AddSingleton(dispatcher);
            }

            if (queryDispatcher is not null)
            {
                services.RemoveAll<IQueryDispatcher>();
                services.AddSingleton(queryDispatcher);
            }
        });
    }
}
