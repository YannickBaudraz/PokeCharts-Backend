using Moq;
using PokeCharts.Controllers;
using PokeCharts.Daos;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests.Controllers;

[TestFixture]
public class PokemonControllerTest
{
    private PokemonsController _pokemonsController;
    private Mock<IPokemonDao> _pokemonDaoMock;

    [SetUp]
    public void OneTimeSetUp()
    {
        _pokemonDaoMock = new Mock<IPokemonDao>();
        _pokemonsController = new PokemonsController(_pokemonDaoMock.Object);
    }

    [Test]
    public void GetAll_NominalCase_Success()
    {
        //given
        Stats stats = new(1, 2, 3, 4, 5, 6);
        Type[] types = new[] { new Type(2, "fire"), new Type(1, "water") };
        PokemonSprites sprites = new("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png",
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/25.png");
        List<Pokemon> pokemons = new()
        {
            new Pokemon(1, "squirtle", 7, 69, sprites, stats, types),
            new Pokemon(4, "charmander", 7, 52, sprites, stats, types),
            new Pokemon(7, "squirtle", 7, 64, sprites, stats, types),
            new Pokemon(25, "pikachu", 7, 55, sprites, stats, types)
        };

        _pokemonDaoMock.Setup(m => m.Get()).Returns(pokemons);

        //when
        List<Pokemon>? results = _pokemonsController.Get().Value;

        //then
        Assert.That(results, Is.EqualTo(pokemons));
    }

    [Test]
    public void GetName_NominalCase_Success()
    {
        //given
        PokemonSprites sprites = new("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png",
        "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/25.png");
        Pokemon expectedPokemon = new(25, "pikachu", 7, 55, sprites, new Stats(1, 2, 3, 4, 5, 6), new[] { new Type(2, "fire"), new Type(1, "water") });

        _pokemonDaoMock.Setup(m => m.Get("pikachu")).Returns(expectedPokemon);

        //when
        Pokemon? results = _pokemonsController.Get("pikachu").Value;
        
        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }

    [Test]
    public void GetName_PokemonDoesNotExist_Exception()
    {
        //given
        _pokemonDaoMock.Setup(m => m.Get("teemo")).Throws(new Exception("pokemon does not exist"));

        //when & then
        Assert.Throws<Exception>(() => _pokemonsController.Get("teemo"));
    }

    [Test]
    public void GetId_NominalCase_Success()
    {
        //given
        PokemonSprites sprites = new("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png",
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/25.png");
        Pokemon expectedPokemon = new(25, "pikachu", 7, 55, sprites, new Stats(1, 2, 3, 4, 5, 6), new[] { new Type(2, "fire"), new Type(1, "water") });

        _pokemonDaoMock.Setup(m => m.Get(25)).Returns(expectedPokemon);

        //when
        Pokemon? results = _pokemonsController.Get(25).Value;

        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }

    [Test]
    public void GetId_PokemonDoesNotExist_Exception()
    {
        //given
        _pokemonDaoMock.Setup(m => m.Get(-1)).Throws(new Exception("pokemon does not exist"));

        //when & then
        Assert.Throws<Exception>(() => _pokemonsController.Get(-1));
    }
    [Test]
    public void GetNames_NominalCase_Success()
    {

        //given
        List<string> names = new() { "pikachu", "charmander", "squirtle" };
        _pokemonDaoMock.Setup(m => m.GetNames()).Returns(names);

        //when
         List<string>? results = _pokemonsController.GetNames().Value;
        //then
        Assert.That(results, Is.EqualTo(names));
    }
}