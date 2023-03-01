using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using PokeCharts.Daos;
using PokeCharts.Exceptions;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.IntegrationTests.Daos;

public class PokemonDaoIntegrationTests
{
    PokemonDao _pokemonDao;
    IConfigurationRoot _configuration;

    [SetUp]
    public void OneTimeSetUp()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _pokemonDao = new PokemonDao(_configuration);
    }

    [Test]
    public void Get_ById_NominalCase_ReturnsPokemon()
    {
        //given
        string urlSuffix = _configuration.GetValue<string>("GraphQl:SpriteSuffix")
                           ?? throw new ArgumentException("The GraphQL sprite suffix is not configured");

        string mainUrl = urlSuffix + "25.png";
        string shinyUrl = urlSuffix + "shiny/25.png";
        Pokemon expectedPokemon = new Pokemon(25, "pikachu", 4f, 60f, new PokemonSprites(mainUrl, shinyUrl), new Stats(35, 55, 40, 50, 50, 90),
            new Type[] { new Type(13, "electric") });

        //when
        object results = _pokemonDao.Get(expectedPokemon.Id);
        //then
        Assert.That(expectedPokemon.ToJson(), Is.EqualTo(results.ToJson()));
    }

    [Test]
    public void Get_ByName_NominalCase_ReturnsPokemon()
    {
        //given
        string urlSuffix = _configuration.GetValue<string>("GraphQl:SpriteSuffix")
                           ?? throw new ArgumentException("The GraphQL sprite suffix is not configured");

        string mainUrl = urlSuffix + "25.png";
        string shinyUrl = urlSuffix + "shiny/25.png";
        Pokemon expectedPokemon = new Pokemon(25, "pikachu", 4f, 60f, new PokemonSprites(mainUrl, shinyUrl), new Stats(35, 55, 40, 50, 50, 90),
            new Type[] { new Type(13, "electric") });

        //when
        object results = _pokemonDao.Get(expectedPokemon.Name);
        //then
        Assert.That(expectedPokemon.ToJson(), Is.EqualTo(results.ToJson()));
    }

    [Test]
    public void Get_ById_WrongId_ThrowsException()
    {
        //given
        int id = -1;
        //then
        Assert.Throws<PokemonNotFoundException>(() => _pokemonDao.Get(id));
    }

    [Test]
    public void Get_ByName_WrongName_ThrowsException()
    {
        //given
        string name = "picachu";
        //then
        Assert.Throws<PokemonNotFoundException>(() => _pokemonDao.Get(name));
    }

    [Test]
    public void Get_NominalCase_ReturnsAllPokemons()
    {
        //given
        //load the json file from the resources folder
        string expectedList = System.IO.File.ReadAllText(@"..\..\..\..\PokeCharts.UnitTests\Resources\PokemonList.json");

        //when
        List<Pokemon> pokemons = _pokemonDao.Get();
        string actualList = pokemons.ToJson();
        //then
        Assert.That(actualList, Is.EqualTo(expectedList));
    }
}