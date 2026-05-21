using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Application.Extensions;
using ViaEventAssociation.Core.Domain.Aggregates.EventAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.GuestAggregate;
using ViaEventAssociation.Core.Domain.Aggregates.LocationAggregate;
using ViaEventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Contexts;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.Repositories;
using ViaEventAssociation.Infrastructure.EfcDmPersistence.UnitOfWork;
using ViaEventAssociation.Infrastructure.EfcQueries.Contexts;
using ViaEventAssociation.Infrastructure.EfcQueries.Extensions;
using ViaEventAssociation.Infrastructure.EfcQueries.Seed;
using ViaEventAssociation.Presentation.WebAPI.Mapping;

var builder = WebApplication.CreateBuilder(args);

var writeConnectionString = builder.Configuration.GetConnectionString("ViaEventAssociationWrite")
    ?? "Data Source=ViaEventAssociation.Write.db";
var queryConnectionString = builder.Configuration.GetConnectionString("ViaEventAssociationQuery")
    ?? "Data Source=ViaEventAssociation.Query.db";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DmContext>(options => options.UseSqlite(writeConnectionString));
builder.Services.AddDbContext<QueryDbContext>(options => options.UseSqlite(queryConnectionString));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IUnitOfWork, EfcUnitOfWork>();
builder.Services.AddScoped<IDispatcher, CommandDispatcher>();
builder.Services.AddCommandHandlers();
builder.Services.AddEfcQueries();
builder.Services.AddObjectMapper();
builder.Services.AddPresentationMappings();

var app = builder.Build();

var shouldInitializeDatabases =
    app.Environment.IsDevelopment()
    || app.Environment.IsEnvironment("Testing")
    || builder.Configuration.GetValue<bool>("InitializeDatabasesOnStartup");

if (shouldInitializeDatabases)
    await InitializeDatabasesAsync(app.Services);

app.MapControllers();

app.Run();

static async Task InitializeDatabasesAsync(IServiceProvider services)
{
    await using var scope = services.CreateAsyncScope();

    var writeContext = scope.ServiceProvider.GetRequiredService<DmContext>();
    await writeContext.Database.EnsureCreatedAsync();

    var queryContext = scope.ServiceProvider.GetRequiredService<QueryDbContext>();
    await queryContext.Database.EnsureCreatedAsync();

    var seedDirectory = FindAssignmentEightSeedDirectory();
    if (seedDirectory is not null)
        await queryContext.SeedFromJsonDirectoryAsync(seedDirectory);
}

static string? FindAssignmentEightSeedDirectory()
{
    var current = new DirectoryInfo(AppContext.BaseDirectory);

    while (current is not null)
    {
        var candidate = Path.Combine(current.FullName, "Assignments", "Assignment8", "ViaEventAssociation");
        if (File.Exists(Path.Combine(candidate, "Events.json")))
            return candidate;

        current = current.Parent;
    }

    return null;
}

public partial class Program;
