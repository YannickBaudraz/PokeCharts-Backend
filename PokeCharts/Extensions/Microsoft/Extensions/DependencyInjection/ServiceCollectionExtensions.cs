namespace PokeCharts.Extensions.Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCors(this IServiceCollection services, IEnumerable<string> allowedOrigins)
    {
        services.AddCors(options => options.AddDefaultPolicy(policy => policy
            .SetIsOriginAllowed(origin => allowedOrigins.Any(origin.StartsWith))
            .AllowAnyHeader()
            .AllowAnyMethod()));

        return services;
    }
}