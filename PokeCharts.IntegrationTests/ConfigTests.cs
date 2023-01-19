using Microsoft.Extensions.Configuration;
using PokeCharts.IntegrationTests.Helpers;

namespace PokeCharts.IntegrationTests;

[TestFixture]
public class ConfigTests
{
    private IConfiguration _config;

    [SetUp]
    public void Setup() => _config = ConfigTestHelper.Configuration;

    [Test]
    public void Section_OnlyInDefaultFile_NotEmpty()
    {
        // Given
        const string graphQlSection = "GraphQl";

        // When
        IConfigurationSection configurationSection = _config.GetSection(graphQlSection);

        // Then
        Assert.That(configurationSection.GetChildren(), Is.Not.Empty);
    }

    [Test]
    public void Value_OnlyInTestFile_ExpectedValue()
    {
        // Given
        const int valueOnlyInTestConfig = 443;

        // When
        var value = _config.GetValue<int>("https_port");

        // Then
        Assert.That(value, Is.EqualTo(valueOnlyInTestConfig));
    }

    [Test]
    public void LogLevels_OverrideDefault_DefaultAndOverriden()
    {
        // Given
        const string logLevel = "Logging:LogLevel";
        const string expectedNetCoreLevel = "Warning";
        const string expectedOverridenDefaultLevel = "Error";

        // When
        IConfigurationSection logLevelSection = _config.GetSection(logLevel);

        // Then
        Assert.Multiple(() =>
        {
            var netCoreLevel = logLevelSection.GetValue<string>("Microsoft.AspNetCore");
            Assert.That(netCoreLevel, Is.EqualTo(expectedNetCoreLevel));

            var defaultLevel = logLevelSection.GetValue<string>("Default");
            Assert.That(defaultLevel, Is.EqualTo(expectedOverridenDefaultLevel));
        });
    }

    [Test]
    public void Key_NonExistentKey_Null()
    {
        // Given
        const string nonExistentKey = "NonExistentKey";

        // When
        string? key = _config[nonExistentKey];

        // Then
        Assert.That(key, Is.Null);
    }
}