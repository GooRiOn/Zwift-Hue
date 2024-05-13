using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZwiftHue.Core.Infrastructure.Zwift;

public static class Extensions
{
    private const string SectionName = "Zwift";
    
    public static IServiceCollection AddZwift(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ZwiftOptions>(configuration.GetSection(SectionName));
        services.AddHttpClient<ZwiftClient>();
        services.AddSingleton<ZwiftAuthData>();
        services.AddHostedService<ZwiftActivityWorker>();
        return services;
    }
}