using Microsoft.VisualBasic;
using Moq;
using PokeCharts.Controllers;
using PokeCharts.Daos;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests.Controllers;

public class PokemonControllerTest
{
    PokemonsController pokemonsController;
    Mock<IPokemonDao> pokemonDaoMock;

    [SetUp]
    public void OneTimeSetUp()
    {
        pokemonDaoMock = new Mock<IPokemonDao>();
        pokemonsController = new PokemonsController(pokemonDaoMock.Object);
    }

    [Test]
    public void GetAllNominalCaseSuccess()
    {
        //given
        Stats stats = new(1, 2, 3, 4, 5, 6);
        Type[] types = new[] { new Type(2,"fire"), new Type(1,"water") };
        List<Pokemon> pokemons = new()
        {
            new Pokemon(1, "squirtle", 7, 69,
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/1.png",
                stats,types),
            new Pokemon(4, "charmander", 7, 52,
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/4.png",
                stats,types),
            new Pokemon(7, "squirtle", 7, 64,
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/7.png",
                stats,types),
            new Pokemon(25, "pikachu", 7, 55,
                "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/25.png",
                stats,types)
        };

        pokemonDaoMock.Setup(m => m.Get()).Returns(pokemons);

        //when
        List<Pokemon>? results = pokemonsController.GetAll().Value;

        //then
        Assert.That(results, Is.EqualTo(pokemons));
    }

    [Test]
    public void GetNameSuccess()
    {
        //given
        Pokemon expectedPokemon = new(25, "pikachu", 7, 55,
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/25.png",
            new Stats(1, 2, 3, 4, 5, 6), new[] { new Type(2,"fire"), new Type(1,"water") });

        pokemonDaoMock.Setup(m => m.Get("pikachu")).Returns(expectedPokemon);

        //when
        Pokemon? results = pokemonsController.Get("pikachu").Value;

        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }

    [Test]
    public void GetNameException()
    {
        //given
        pokemonDaoMock.Setup(m => m.Get("teemo")).Throws(new Exception("pokemon does not exist"));

        //when & then
        Assert.Throws<Exception>(() => pokemonsController.Get("teemo"));
    }

    [Test]
    public void GetIdSuccess()
    {
        //given
        Pokemon expectedPokemon = new(25, "pikachu", 7, 55,
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/official-artwork/25.png",
            new Stats(1, 2, 3, 4, 5, 6), new[] { new Type(2,"fire"), new Type(1,"water") });

        pokemonDaoMock.Setup(m => m.Get(25)).Returns(expectedPokemon);

        //when
        Pokemon? results = pokemonsController.Get(25).Value;

        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }

    [Test]
    public void GetIdException()
    {
        //given
        pokemonDaoMock.Setup(m => m.Get(-1)).Throws(new Exception("pokemon does not exist"));

        //when & then
        Assert.Throws<Exception>(() => pokemonsController.Get(-1));
    }
}