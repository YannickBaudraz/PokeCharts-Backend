using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using PokeCharts.Daos;
using PokeCharts.Exceptions;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.IntegrationTests.Daos;

public class TypeDaoIntegrationTests
{
    private IConfigurationRoot _configuration;


    private readonly int _flyingId = 3;
    private readonly string _fylingName = "flying";
    private PokemonTypeDao _typeDao;

    [SetUp]
    public void OneTimeSetUp()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _typeDao = new PokemonTypeDao(_configuration);
    }

    [Test]
    public void Get_NoParameter_ReturnsAllTypes()
    {
        // given
        // load the json file from the resources folder
        string expectedList = File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/TypesList.json");

        // when
        List<Type> types = _typeDao.Get();
        string actualList = types.ToJson();

        // then
        Assert.That(actualList, Is.EqualTo(expectedList));
    }

    [Test]
    public void Get_ExistingId_ReturnsType()
    {
        // given
        // load the json file from the resources folder
        string expectedList = File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/FlyingType.json");

        // when
        Type types = _typeDao.Get(_flyingId);
        string actualList = types.ToJson();

        // then
        Assert.That(actualList, Is.EqualTo(expectedList));
    }

    [Test]
    public void Get_ExistingName_ReturnsType()
    {
        // given
        // load the json file from the resources folder
        string expectedList = File.ReadAllText(@"../../../../PokeCharts.UnitTests/Resources/FlyingType.json");

        // when
        Type types = _typeDao.Get(_fylingName);
        string actualList = types.ToJson();

        // then
        Assert.That(actualList, Is.EqualTo(expectedList));
    }

    [Test]
    public void Get_NonExistentName_ThrowsException()
    {
        //given
        const string name = "flyiing";

        //then
        Assert.Throws<TypeNotFoundException>(() => _typeDao.Get(name));
    }

    [Test]
    public void Get_NonExistentId_ThrowsException()
    {
        //given
        const int id = -1;

        //then
        Assert.Throws<TypeNotFoundException>(() => _typeDao.Get(id));
    }
}