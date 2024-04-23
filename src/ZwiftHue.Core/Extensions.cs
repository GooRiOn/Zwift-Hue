using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZwiftHue.Core.Commands;
using ZwiftHue.Core.Hue;
using ZwiftHue.Core.Zwift;

namespace ZwiftHue.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddZwift(configuration)
            .AddHue(configuration)
            .AddCommands();
        
        return services;
    }
}