using Microsoft.AspNetCore.Mvc;
using PokeCharts.Daos;
using PokeCharts.Models;

namespace PokeCharts.Controllers;

[Route("[controller]")]
[ApiController]
public class PokemonsController : ControllerBase
{
    private readonly IPokemonDao _pokemonDao;

    public PokemonsController(IPokemonDao pokemonDao)
    {
        _pokemonDao = pokemonDao;
    }

    [HttpGet]
    public ActionResult<List<Pokemon>> GetAll()
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