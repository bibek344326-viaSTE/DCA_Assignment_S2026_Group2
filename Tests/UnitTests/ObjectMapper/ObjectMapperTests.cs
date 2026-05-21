using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.Tools.ObjectMapper;

namespace UnitTests.ObjectMapper;

public class ObjectMapperTests
{
    [Fact]
    public void Map_UsesDefaultPropertyMapping_WhenNoExplicitMappingExists()
    {
        var mapper = CreateMapper();

        var destination = mapper.Map<SourceDto, DestinationDto>(new SourceDto("Friday Bar", 25));

        Assert.Equal("Friday Bar", destination.Title);
        Assert.Equal(25, destination.Count);
    }

    [Fact]
    public void Map_UsesExplicitMapping_WhenMappingIsRegistered()
    {
        var services = new ServiceCollection();
        services.AddObjectMapper();
        services.AddScoped<IMapping<SourceDto, ExplicitDestinationDto>, SourceToExplicitDestinationMapping>();

        var mapper = services.BuildServiceProvider().GetRequiredService<IObjectMapper>();

        var destination = mapper.Map<SourceDto, ExplicitDestinationDto>(new SourceDto("Friday Bar", 25));

        Assert.Equal("Friday Bar (25)", destination.DisplayText);
    }

    private static IObjectMapper CreateMapper()
    {
        var services = new ServiceCollection();
        services.AddObjectMapper();
        return services.BuildServiceProvider().GetRequiredService<IObjectMapper>();
    }

    private sealed record SourceDto(string Title, int Count);

    private sealed record DestinationDto(string Title, int Count);

    private sealed record ExplicitDestinationDto(string DisplayText);

    private sealed class SourceToExplicitDestinationMapping : IMapping<SourceDto, ExplicitDestinationDto>
    {
        public ExplicitDestinationDto Map(SourceDto source)
            => new($"{source.Title} ({source.Count})");
    }
}
