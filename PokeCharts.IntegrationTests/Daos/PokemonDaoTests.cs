using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using PokeCharts.Daos;
using PokeCharts.Exceptions;
using PokeCharts.Models;
using PokeCharts.Models.Dtos;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.IntegrationTests.Daos;

public class PokemonDaoIntegrationTests
{
    private IConfigurationRoot _configuration;
    private PokemonDao _pokemonDao;

    [SetUp]
    public void OneTimeSetUp()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _pokemonDao = new PokemonDao(_configuration, new MoveDao(_configuration), new PokemonTypeDao(_configuration));
    }

    [Test]
    public void Get_ExistingId_ReturnsPokemon()
    {
        //given
        string urlSuffix = _configuration.GetValue<string>("GraphQl:SpriteSuffix")
                           ?? throw new ArgumentException("The GraphQL sprite suffix is not configured");

        string mainUrl = urlSuffix + "25.png";
        string shinyUrl = urlSuffix + "shiny/25.png";
        var expectedPokemon = new Pokemon(25, "pikachu", 4f, 60f, new PokemonSprites(mainUrl, shinyUrl),
            new Stats(35, 55, 40, 50, 50, 90),
            new[] { new Type(13, "electric") });

        //when
        object results = _pokemonDao.Get(expectedPokemon.Id);

        //then
        Assert.That(expectedPokemon.ToJson(), Is.EqualTo(results.ToJson()));
    }

    [Test]
    public void Get_ExistingName_ReturnsPokemon()
    {
        //given
        string urlSuffix = _configuration.GetValue<string>("GraphQl:SpriteSuffix")
                           ?? throw new ArgumentException("The GraphQL sprite suffix is not configured");

        string mainUrl = urlSuffix + "25.png";
        string shinyUrl = urlSuffix + "shiny/25.png";
        var expectedPokemon = new Pokemon(25, "pikachu", 4f, 60f, new PokemonSprites(mainUrl, shinyUrl),
            new Stats(35, 55, 40, 50, 50, 90),
            new[] { new Type(13, "electric") });

        //when
        object results = _pokemonDao.Get(expectedPokemon.Name);

        //then
        Assert.That(expectedPokemon.ToJson(), Is.EqualTo(results.ToJson()));
    }

    [Test]
    public void Get_NonExistentId_ThrowsException()
    {
        //given
        const int id = -1;

        //then
        Assert.Throws<PokemonNotFoundException>(() => _pokemonDao.Get(id));
    }

    [Test]
    public void Get_NonExistentName_ThrowsException()
    {
        //given
        const string name = "picachu";

        //then
        Assert.Throws<PokemonNotFoundException>(() => _pokemonDao.Get(name));
    }

    [Test]
    public void Get_NoParameter_ReturnsAllPokemons()
    {
        //given
        string expectedList = File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/PokemonList.json");

        //when
        List<Pokemon> pokemons = _pokemonDao.Get();
        string actualList = pokemons.ToJson();

        //then
        Assert.That(actualList, Is.EqualTo(expectedList));
    }

    [Test]
    public void GetLight_NominalCase_ReturnsAllPokemonNames()
    {
        //given
        string expectedList = File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/PokemonNameList.json");

        //when
        List<PokemonLightDto> pokemons = _pokemonDao.GetLights();

        //then
        string actualList = pokemons.Select(p => p.Name).ToJson();
        Assert.That(actualList, Is.EqualTo(expectedList));
    }

    [Test]
    public void GetDamage_NominalCase_ReturnsDamage()
    {
        //given
        const int attackerId = 1;
        const int defenderId = 2;
        const int moveId = 1;
        const float expectedDamage = 13.6888895f;
        const float expectedMultiplier = 1f;

        //when
        List<float> actualDamage = _pokemonDao.GetDamage(attackerId, defenderId, moveId);

        //then
        Assert.Multiple(() =>
        {
            Assert.That(actualDamage[0], Is.EqualTo(expectedDamage));
            Assert.That(actualDamage[1], Is.EqualTo(expectedMultiplier));
        });
    }
}