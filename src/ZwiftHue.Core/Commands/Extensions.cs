using Microsoft.Extensions.DependencyInjection;

namespace ZwiftHue.Core.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assembly = typeof(ICommand).Assembly;

        services.Scan(x => x.FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}