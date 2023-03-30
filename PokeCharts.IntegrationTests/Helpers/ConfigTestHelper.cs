using Microsoft.Extensions.Configuration;

namespace PokeCharts.IntegrationTests.Helpers;

internal static class ConfigTestHelper
{
    public static IConfiguration Configuration { get; } =
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.IntegrationTests.json")
            .AddEnvironmentVariables()
            .Build();
}