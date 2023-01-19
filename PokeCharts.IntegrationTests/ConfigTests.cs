using Microsoft.Extensions.Configuration;
using PokeCharts.IntegrationTests.Helpers;

namespace PokeCharts.IntegrationTests;

[TestFixture]
public class ConfigTests
{
    [Test]
    public void Configuration_ValueOnlyInTestConfigFile_CanAccessValue()
    {
        // Given
        const int expectedValue = 443;

        // When
        IConfiguration config = ConfigTestHelper.Configuration;

        // Then
        var value = config.GetValue<int>("https_port");
        Assert.That(value, Is.EqualTo(expectedValue));
    }
}