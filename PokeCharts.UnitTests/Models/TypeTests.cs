using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests.Model;

[TestFixture]
public class TypeTests
{
    private readonly Type _normal = new(1, "normal");
    private readonly Type _fire = new(10, "fire");
    private readonly Type _flying = new(3, "fyling");

    [Test]
    public void Constructor_DamagePropertiesAsParameter_Success()
    {
        // Given
        const int id = 6;
        const string name = "rock";
        var halfDamage = new List<Type> { _normal };
        var doubleDamage = new List<Type> { _flying };
        var noDamageTo = new List<Type> { _fire };

        // When
        // Then
        Assert.DoesNotThrow(() =>
        {
            var type = new Type(id, name, doubleDamage, halfDamage, noDamageTo);
        });
    }

    [Test]
    public void Constructor_DamagePropertiesTypeConflict_ThrowsException()
    {
        // Given
        const int id = 6;
        const string name = "rock";
        var halfDamage = new List<Type> { _normal, _fire };
        var doubleDamage = new List<Type> { _normal, _flying };

        // When
        // Then
        Assert.Throws<Exception>(() => { new Type(id, name, doubleDamage, halfDamage, new List<Type>()); });
    }

    [Test]
    public void AddDamageProperties_NominalCase_Success()
    {
        // Given
        const int id = 6;
        const string name = "rock";
        var halfDamage = new List<Type> { _normal };
        var doubleDamage = new List<Type> { _flying };
        var noDamageTo = new List<Type> { _fire };

        // When
        // Then
        Assert.DoesNotThrow(() =>
        {
            var type = new Type(id, name, doubleDamage, halfDamage, noDamageTo);
        });
    }

    [Test]
    public void AddDamageProperties_TypeConflict_ThrowException()
    {
        // Given
        const int id = 6;
        const string name = "rock";
        var halfDamage = new List<Type> { _normal };
        var doubleDamage = new List<Type> { _normal, _flying };
        var noDamageTo = new List<Type> { _fire };

        // When
        // Then
        Assert.Throws<Exception>(() =>
        {
            var type = new Type(id, name, doubleDamage, halfDamage, noDamageTo);
        });
    }
}