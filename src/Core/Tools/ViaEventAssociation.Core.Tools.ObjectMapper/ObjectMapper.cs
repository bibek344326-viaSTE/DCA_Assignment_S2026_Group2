using System.Reflection;

namespace ViaEventAssociation.Core.Tools.ObjectMapper;

public sealed class ObjectMapper(IServiceProvider serviceProvider) : IObjectMapper
{
    public TDestination Map<TSource, TDestination>(TSource source)
        where TSource : notnull
    {
        var explicitMapping = serviceProvider.GetService(typeof(IMapping<TSource, TDestination>));
        if (explicitMapping is IMapping<TSource, TDestination> mapping)
            return mapping.Map(source);

        return DefaultMap<TSource, TDestination>(source);
    }

    private static TDestination DefaultMap<TSource, TDestination>(TSource source)
    {
        var sourceProperties = typeof(TSource)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead)
            .ToDictionary(property => property.Name, StringComparer.OrdinalIgnoreCase);

        var destinationType = typeof(TDestination);
        var constructor = destinationType
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
            .OrderByDescending(ctor => ctor.GetParameters().Length)
            .FirstOrDefault();

        object destination;
        if (constructor is not null && constructor.GetParameters().Length > 0)
        {
            var arguments = constructor.GetParameters()
                .Select(parameter => ReadSourceValue(source, sourceProperties, parameter.Name!, parameter.ParameterType))
                .ToArray();

            destination = constructor.Invoke(arguments);
        }
        else
        {
            destination = Activator.CreateInstance(destinationType)
                ?? throw new InvalidOperationException($"Could not create {destinationType.Name}.");
        }

        foreach (var destinationProperty in destinationType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!destinationProperty.CanWrite)
                continue;

            var value = ReadSourceValue(source, sourceProperties, destinationProperty.Name, destinationProperty.PropertyType);
            destinationProperty.SetValue(destination, value);
        }

        return (TDestination)destination;
    }

    private static object? ReadSourceValue<TSource>(
        TSource source,
        IReadOnlyDictionary<string, PropertyInfo> sourceProperties,
        string name,
        Type destinationType)
    {
        if (!sourceProperties.TryGetValue(name, out var sourceProperty))
            return destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;

        var value = sourceProperty.GetValue(source);
        if (value is null)
            return null;

        if (destinationType.IsAssignableFrom(sourceProperty.PropertyType))
            return value;

        var underlyingType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
        return Convert.ChangeType(value, underlyingType);
    }
}
