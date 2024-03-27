using Microsoft.Extensions.Configuration;

namespace BrickManager.BrickRecognitionSystem.Api;

public static class ConfigurationExtensions
{
    public static void BindFromSection<TOptions>(this IConfiguration configuration, TOptions options)
        => configuration.GetSection(typeof(TOptions).Name).Bind(options);
}