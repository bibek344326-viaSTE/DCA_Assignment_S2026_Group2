namespace ViaEventAssociation.Core.Tools.ObjectMapper;

public interface IMapping<in TSource, out TDestination>
    where TSource : notnull
{
    TDestination Map(TSource source);
}
