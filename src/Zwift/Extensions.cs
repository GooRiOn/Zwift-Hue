namespace ZwiftHue.Zwift;

public static class Extensions
{
    private const string SectionName = "Zwift";
    
    public static IServiceCollection AddZwift(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ZwiftOptions>(configuration.GetSection(SectionName));
        services.AddHttpClient<ZwiftClient>();
        return services;
    }
}