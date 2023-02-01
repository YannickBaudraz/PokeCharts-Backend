using PokeCharts.Models;
using Moq;
using PokeCharts.Daos;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.UnitTests;

public class PokemonDaoUnitTests
{
    PokemonDao pokemonDao;
    [SetUp]
    public void OneTimeSetUp()
    {
        pokemonDao = new PokemonDao();
    }

    [Test]
    public void GetNominalCaseSuccess()
    {
        //given
        Pokemon expectedPokemon = new Pokemon(25, "pikachu", 0.4f, 6f, "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png", new Stats(35, 55, 40, 50, 50, 90), new Type[] { new Type(13, "electric") });
        //when
        object results = pokemonDao.Get(25);
        //then
        Assert.That(results, Is.EqualTo(expectedPokemon));
    }
    
}