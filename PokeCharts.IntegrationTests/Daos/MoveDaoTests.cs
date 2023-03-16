using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using PokeCharts.Daos;
using PokeCharts.Exceptions;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.IntegrationTests.Daos;

public class MoveDaoIntegrationTests
{
    PokemonMoveDao _moveDao;
    IConfigurationRoot _configuration;

    [SetUp]
    public void OneTimeSetUp()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _moveDao = new PokemonMoveDao(_configuration);
    }

    [Test]
    public void Get_ExistingId_ReturnsMoves()
    {
        //given
        int id = 25;
        string expectedList = System.IO.File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/PikachuMoves.json");

        //when
        object results = _moveDao.Get(id);
        //then
        Assert.That(expectedList, Is.EqualTo(results.ToJson()));
    }
    [Test]
    public void Get_ExistingName_ReturnsMoves()
    {
        //given
        string name = "pikachu";
        string expectedList = System.IO.File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/PikachuMoves.json");

        //when
        object results = _moveDao.Get(name).ToJson();
        //then
        Assert.That(expectedList, Is.EqualTo(results));
    }

    [Test]
    public void Get_NonExistentId_ThrowsException()
    {
        //given
        int id = -1;
        //then
        Assert.Throws<PokemonNotFoundException>(() => _moveDao.Get(id));
    }

    [Test]
    public void Get_NonExistentName_ThrowsException()
    {
        //given
        string name = "picachu";
        //then
        Assert.Throws<PokemonNotFoundException>(() => _moveDao.Get(name));
    }
}