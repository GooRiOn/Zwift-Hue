using Microsoft.Extensions.DependencyInjection;

namespace ZwiftHue.Core.Infrastructure.ProfileConfigs;

internal static class Extensions
{
    public static IServiceCollection AddRiderConfigurations(this IServiceCollection services)
    {
        services.AddSingleton<IRiderProfileConfigurationProvider, RiderProfileConfigurationProvider>();
        return services;
    }
}