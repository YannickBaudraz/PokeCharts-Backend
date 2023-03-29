namespace PokeCharts.Extensions.Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds CORS to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="allowedOrigins"></param>
    /// <returns></returns>
    public static IServiceCollection AddCors(this IServiceCollection services, IEnumerable<string> allowedOrigins)
    {
        services.AddCors(options => options.AddDefaultPolicy(policy => policy
            .SetIsOriginAllowed(origin => allowedOrigins.Any(origin.StartsWith))
            .AllowAnyHeader()
            .AllowAnyMethod()));

        return services;
    }
}