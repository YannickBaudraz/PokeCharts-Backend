using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests.Model;

[TestFixture]
public class PokemonTests
{
    // Required attributes to create a pokemon
    private readonly int _id = 6;
    private readonly string _name = "charizard";
    private readonly float _height = 17;
    private readonly float _weight = 905;
    private readonly PokemonSprites _sprites = new("25.png", "shiny/25.png");
    private readonly Stats _stat = new(78, 84, 78, 109, 85, 100);
    private readonly Type _firstType = new(10, "fire");
    private readonly Type _secondType = new(3, "flying");
    private readonly Type _thirdType = new(100, "poison");


    [Test]
    public void Constructor_NominalCase_Success()
    {
        // Given
        // Refer to class attributes
        Type[] pokemonTypes = { _firstType, _secondType };

        // When 
        // Then
        Assert.DoesNotThrow(() =>
        {
            var pokemon = new Pokemon(_id, _name, _height, _weight, _sprites, _stat, pokemonTypes);
        });
    }

    [Test]
    public void Constructor_PokemonHasNoType_ThrowsException()
    {
        // Given
        // Refer to class attributes
        Type[] pokemonTypes = { };

        // When 
        // Then

        Assert.Throws<ArgumentException>(() =>
        {
            var pokemon = new Pokemon(_id, _name, _height, _weight, _sprites, _stat, pokemonTypes);
        });
    }

    [Test]
    public void Constructor_PokemonHasMoreThanTwoTypes_ThrowsException()
    {
        // Given
        // Refer to class attributes
        Type[] pokemonTypes = { _firstType, _secondType, _thirdType };

        // When 
        // Then

        Assert.Throws<ArgumentException>(() =>
        {
            var pokemon = new Pokemon(_id, _name, _height, _weight, _sprites, _stat, pokemonTypes);
        });
    }
}