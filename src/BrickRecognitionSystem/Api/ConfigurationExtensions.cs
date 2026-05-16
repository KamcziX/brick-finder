using Microsoft.Extensions.Configuration;

namespace BrickManager.BrickRecognitionSystem.Api;

/// <summary>
/// Extension methods for binding strongly-typed options from application configuration.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Binds a configuration section whose name matches <typeparamref name="TOptions"/> to the provided options instance.
    /// </summary>
    /// <typeparam name="TOptions">The options type. The section name is derived from the type name.</typeparam>
    /// <param name="configuration">The configuration to read from.</param>
    /// <param name="options">The options instance to bind into.</param>
    public static void BindFromSection<TOptions>(this IConfiguration configuration, TOptions options)
        => configuration.GetSection(typeof(TOptions).Name).Bind(options);
}
