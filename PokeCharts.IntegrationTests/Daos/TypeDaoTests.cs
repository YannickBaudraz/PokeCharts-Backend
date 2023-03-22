using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using PokeCharts.Daos;
using PokeCharts.Exceptions;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;


namespace PokeCharts.IntegrationTests.Daos;

public class TypeDaoIntegrationTests
{
    PokemonTypeDao _typeDao;
    IConfigurationRoot _configuration;

    [SetUp]
    public void OneTimeSetUp()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _typeDao = new PokemonTypeDao(_configuration);
    }

}