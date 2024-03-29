using Microsoft.AspNetCore.Mvc;
using Moq;
using PokeCharts.Controllers;
using PokeCharts.Daos;
using PokeCharts.Models;
using PokeCharts.Models.Dtos;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests.Controllers;

[TestFixture]
public class PokemonControllerTest
{
    [SetUp]
    public void OneTimeSetUp()
    {
        _pokemonDaoMock = new Mock<IPokemonDao>();
        _pokemonsController = new PokemonsController(_pokemonDaoMock.Object);
    }

    private PokemonsController _pokemonsController;
    private Mock<IPokemonDao> _pokemonDaoMock;

    [Test]
    public void Get_NoParameter_ReturnsAllPokemons()
    {
        //given
        Stats stats = new(1, 2, 3, 4, 5, 6);
        Type[] types = { new Type(2, "fire"), new Type(1, "water") };
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
        var results = _pokemonsController.Get().Result as OkObjectResult;

        //then
        Assert.That(results?.Value, Is.EqualTo(pokemons));
    }

    [Test]
    public void Get_ExistingName_ReturnsOnePokemon()
    {
        //given
        PokemonSprites sprites = new("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png",
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/25.png");

        Pokemon expectedPokemon = new(25, "pikachu", 7, 55, sprites, new Stats(1, 2, 3, 4, 5, 6),
            new[] { new Type(2, "fire"), new Type(1, "water") });

        _pokemonDaoMock.Setup(m => m.Get("pikachu")).Returns(expectedPokemon);

        //when
        Pokemon? results = _pokemonsController.Get("pikachu").Value;

        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }

    [Test]
    public void Get_NonExistentName_ThrowsException()
    {
        //given
        _pokemonDaoMock.Setup(m => m.Get("teemo")).Throws(new Exception("pokemon does not exist"));

        //when & then
        Assert.Throws<Exception>(() => _pokemonsController.Get("teemo"));
    }

    [Test]
    public void Get_ExistingId_ReturnsOnePokemon()
    {
        //given
        PokemonSprites sprites = new("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png",
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/shiny/25.png");

        Pokemon expectedPokemon = new(25, "pikachu", 7, 55, sprites, new Stats(1, 2, 3, 4, 5, 6),
            new[] { new Type(2, "fire"), new Type(1, "water") });

        _pokemonDaoMock.Setup(m => m.Get(25)).Returns(expectedPokemon);

        //when
        Pokemon? results = _pokemonsController.Get(25).Value;

        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }

    [Test]
    public void Get_NonExistentId_ThrowsException()
    {
        //given
        _pokemonDaoMock.Setup(m => m.Get(-1)).Throws(new Exception("pokemon does not exist"));

        //when & then
        Assert.Throws<Exception>(() => _pokemonsController.Get(-1));
    }

    [Test]
    public void Get_LightParameter_ReturnsPokemonLight()
    {
        //given
        List<string> names = new() { "pikachu", "charmander", "squirtle" };
        _pokemonDaoMock
            .Setup(m => m.GetLights())
            .Returns(names.Select(name => new PokemonLightDto(name)).ToList);

        //when
        var result = _pokemonsController.Get(true).Result as ObjectResult;
        var values = result!.Value as List<PokemonLightDto>;

        //then
        Assert.That(values, Is.InstanceOf(typeof(List<PokemonLightDto>)));

        List<string> resultNames = values!.Select(r => r.Name).ToList();
        Assert.That(resultNames, Is.EqualTo(names));
    }
}