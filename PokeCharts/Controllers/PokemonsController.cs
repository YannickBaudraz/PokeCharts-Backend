using Microsoft.AspNetCore.Mvc;
using PokeCharts.Daos;
using PokeCharts.Models;

namespace PokeCharts.Controllers;

[Route("[controller]")]
[ApiController]
public class PokemonsController : ControllerBase
{
    private readonly IPokemonDao _pokemonDao;

    public PokemonsController(IPokemonDao? pokemonDao = null)
    {
        _pokemonDao = pokemonDao ?? new PokemonDao(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build());
    }

    [HttpGet]
    public ActionResult<List<Pokemon>> Get()
    {
        return _pokemonDao.Get();
    }

    [HttpGet("{id:int}")]
    public ActionResult<Pokemon> Get(int id)
    {
        return _pokemonDao.Get(id);
    }

    [HttpGet("{name}")]
    public ActionResult<Pokemon> Get(string name)
    {
        return _pokemonDao.Get(name);
    }
}