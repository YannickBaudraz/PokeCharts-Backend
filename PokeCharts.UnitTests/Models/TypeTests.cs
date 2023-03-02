using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests.Model;

[TestFixture]
public class TypeTests
{   
    private Type _normal = new Type(1, "normal");
    private Type _fire = new Type(10, "fire");
    private Type _flying =  new Type(3, "fyling");

    [Test]
    public void Constructor_DamagePropertiesAsParameter_Success()
    {
        // Given
        int id = 6;
        string name = "rock";
        List<Type> halfDamage = new List<Type> {_normal};
        List<Type> doubleDamage = new List<Type> {_flying};
        List<Type> noDamageTo = new List<Type> {_fire};

        // When
        // Then
        Assert.DoesNotThrow(() => {
            Type type = new Type(id, name, doubleDamage, halfDamage,noDamageTo);
        });
    }

    [Test]
    public void Constructor_DamagePropertiesTypeConflict_ThrowsException()
    {
        // Given
        int id = 6;
        string name = "rock";
        List<Type> halfDamage = new List<Type> {_normal, _fire};
        List<Type> doubleDamage = new List<Type> {_normal, _flying};

        // When
        // Then
        Assert.Throws<Exception>(() => {
            new Type(id, name, doubleDamage, halfDamage, new List<Type>());
        });        
    }

    [Test]
    public void AddDamageProperties_NominalCase_Success()
    {
        // Given
        int id = 6;
        string name = "rock";
        List<Type> halfDamage = new List<Type> {_normal};
        List<Type> doubleDamage = new List<Type> {_flying};
        List<Type> noDamageTo = new List<Type> {_fire};

        // When
        // Then
        Assert.DoesNotThrow(() => {
            Type type = new Type(id, name, doubleDamage, halfDamage,noDamageTo);
        });
    }

    [Test]
    public void AddDamageProperties_TypeConflict_ThrowException()
    {
        // Given
        int id = 6;
        string name = "rock";
        List<Type> halfDamage = new List<Type> {_normal};
        List<Type> doubleDamage = new List<Type> {_normal, _flying};
        List<Type> noDamageTo = new List<Type> {_fire};

        // When
        // Then
        Assert.Throws<Exception>(() => {
            Type type = new Type(id, name, doubleDamage, halfDamage,noDamageTo);
        });
    }

}
