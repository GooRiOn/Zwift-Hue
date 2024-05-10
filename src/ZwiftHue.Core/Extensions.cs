using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZwiftHue.Core.Commands;
using ZwiftHue.Core.Infrastructure.Channels;
using ZwiftHue.Core.Infrastructure.Hue;
using ZwiftHue.Core.Infrastructure.Zwift;
using ZwiftHue.Core.Queries;

namespace ZwiftHue.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddZwift(configuration)
            .AddHue(configuration)
            .AddCommands()
            .AddQueries()
            .AddChannel();
        
        return services;
    }
}