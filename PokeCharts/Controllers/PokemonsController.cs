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
    public ActionResult<List<Pokemon>> Get() => _pokemonDao.Get();

    [HttpGet("{id:int}")]
    public ActionResult<Pokemon> Get(int id) => _pokemonDao.Get(id);

    [HttpGet("{name}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<Pokemon> Get(string name) => _pokemonDao.Get(name);
    [HttpGet("Names")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<List<string>> GetNames() => _pokemonDao.GetNames();

    [HttpGet("Filter")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
public ActionResult<List<Pokemon>> GetFiltered(string types, string stat, string conditions, int? conditionValue) => 
        _pokemonDao.GetFiltered(types, stat, conditions, conditionValue);
    
        
}