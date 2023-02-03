using Type = PokeCharts.Models.Type;
using Pokemon = PokeCharts.Models.Pokemon;
using Stats = PokeCharts.Models.Stats;

namespace PokeCharts.UnitTests.Model;


[TestFixture]
public class PokemonTests 
{
    // Required attribute to declare a pokemon
    int _id = 6;
    string _name = "charizard";
    float _height = 17;
    float _weight = 905;
    string _sprite = "6.png";
    Stats _stat = new Stats(78, 84, 78, 109, 85, 100);
    Type _firstType = new Type(10, "fire");
    Type _secondType = new Type(3, "flying");
    Type _thirdType = new Type(100, "poison");


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

