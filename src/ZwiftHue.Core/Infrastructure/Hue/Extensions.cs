using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZwiftHue.Core.Infrastructure.Hue;

public static class Extensions
{
    private const string SectionName = "Hue";

    
    public static IServiceCollection AddHue(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HueOptions>(configuration.GetSection(SectionName));
        services.AddHttpClient<HueClient>();
        return services;
    }
}