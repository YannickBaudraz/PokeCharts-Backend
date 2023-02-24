using System.Diagnostics.CodeAnalysis;
using PokeCharts.Models;

namespace PokeCharts.UnitTests.Models;

[TestFixture]
public class ModelReferenceTests
{
    [Test]
    public void Constructor_IdIsString_ParameterIsName()
    {
        // Given
        const string id = "pikachu";

        // When
        var modelReference = new ModelReference(id);

        // Then
        Assert.That(modelReference.Parameter, Is.EqualTo("name"));
    }

    [Test]
    public void Constructor_IdIsInt_ParameterIsId()
    {
        // Given
        const int id = 25;

        // When
        var modelReference = new ModelReference(id);

        // Then
        Assert.That(modelReference.Parameter, Is.EqualTo("id"));
    }

    [Test]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public void Constructor_IdIsNotStringOrInt_ThrowsException()
    {
        // Given
        const double id = 25.0;

        // When
        void TestDelegate() => new ModelReference(id);

        // Then
        Assert.That(TestDelegate, Throws.ArgumentException);
    }
}