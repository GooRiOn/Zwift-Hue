using Microsoft.Extensions.DependencyInjection;
using ZwiftHue.Core.Commands;

namespace ZwiftHue.Core.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        var assembly = typeof(ICommand).Assembly;

        services.Scan(x => x.FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}