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
    public void ConfigurationSection_OnlyInDefaultFile_NotEmpty()
    {
        // Given
        const string graphQlSection = "GraphQl";

        // When
        IConfigurationSection configurationSection = _config.GetSection(graphQlSection);

        // Then
        Assert.That(configurationSection.GetChildren(), Is.Not.Empty);
    }

    [Test]
    public void ConfigurationKey_NonExistentKey_Null()
    {
        // Given
        const string nonExistentKey = "NonExistentKey";

        // When
        string? key = _config[nonExistentKey];

        // Then
        Assert.That(key, Is.Null);
    }

    [Test]
    public void ConfigurationValue_OnlyInTestFile_ExpectedValue()
    {
        // Given
        const int valueOnlyInTestConfig = 443;

        // When
        var value = _config.GetValue<int>("https_port");

        // Then
        Assert.That(value, Is.EqualTo(valueOnlyInTestConfig));
    }
}