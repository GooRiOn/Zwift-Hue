using Microsoft.Extensions.DependencyInjection;

namespace ZwiftHue.Core.Infrastructure.Channels;

public static class Extensions
{
    public static IServiceCollection AddChannel(this IServiceCollection services)
    {
        services.AddSingleton<IMessageChannel, MessageChannel>();
        return services;
    }
}