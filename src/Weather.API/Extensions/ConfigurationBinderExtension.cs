namespace Weather.API.Extensions;

public static class ConfigurationBinderExtension
{
    public static T SafeGet<T>(this IConfiguration configuration)
    {
        var typeName = typeof(T).Name;
        if (configuration.GetChildren().Any((item) => item.Key == typeName))
        {
            configuration = configuration.GetSection(typeName);
        }

        return configuration.Get<T>() ??
            throw new InvalidOperationException("The configuration '" + typeof(T).FullName + "' item doesn't exist.");

    }
}
