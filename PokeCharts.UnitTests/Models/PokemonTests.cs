using Type = PokeCharts.Models.Type;
using Pokemon = PokeCharts.Models.Pokemon;
using Stats = PokeCharts.Models.Stats;

namespace PokeCharts.UnitTests.Model;


[TestFixture]
public class PokemonTests 
{
    // Required attributes to create a pokemon
    private int _id = 6;
    private string _name = "charizard";
    private float _height = 17;
    private float _weight = 905;
    private string _sprite = "6.png";
    private Stats _stat = new Stats(78, 84, 78, 109, 85, 100);
    private Type _firstType = new Type(10, "fire");
    private Type _secondType = new Type(3, "flying");
    private Type _thirdType = new Type(100, "poison");


    [Test]
    public void Constructor_NominalCase_Success()
    {
        // Given
        // Refer to class attributes
        Type[] pokemonTypes = {_firstType, _secondType};

        // When 
        // Then
        Assert.DoesNotThrow(() => {
            Pokemon pokemon = new Pokemon(_id, _name, _height, _weight, _sprite, _stat, pokemonTypes);
        });
    }

    [Test]
    public void Constructor_PokemonHasMoreThanTwoTypes_ThrowException()
    {
        // Given
        // Refer to class attributes
        Type[] pokemonTypes = {_firstType, _secondType, _thirdType};

        // When 
        // Then
        Assert.Throws<Exception>(() => {
            Pokemon pokemon = new Pokemon(_id, _name, _height, _weight, _sprite, _stat, pokemonTypes);
        });
    }

}

