namespace ViaEventAssociation.Core.Tools.ObjectMapper;

public interface IObjectMapper
{
    TDestination Map<TSource, TDestination>(TSource source)
        where TSource : notnull;
}
