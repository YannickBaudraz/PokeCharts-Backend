using System.Net;
using Microsoft.AspNetCore.Mvc;
using PokeCharts.Daos;
using PokeCharts.Models;
using PokeCharts.Requests;

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
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    [ProducesResponseType(typeof(List<Pokemon>), (int)HttpStatusCode.OK)]
    public ActionResult<List<object>> Get([FromQuery] bool light = false, [FromQuery] PokemonsFilter? filter = null)
    {
        bool filterIsNotEmpty = filter is not null && filter.IsNotEmpty();
        if (light && filterIsNotEmpty)
            throw new ArgumentException("Cannot use both light and filter");

        if (light)
            return Ok(_pokemonDao.GetLights());

        if (filterIsNotEmpty)
            return Ok(_pokemonDao.GetFiltered(filter!));

        return Ok(_pokemonDao.Get());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Pokemon> Get(int id) => _pokemonDao.Get(id);

    [HttpGet("{name}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<Pokemon> Get(string name) => _pokemonDao.Get(name);

    [HttpGet("Attack")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<List<float>> Attack(int attackerId, int targetId, int moveId) =>
        _pokemonDao.GetDamage(attackerId, targetId, moveId);
}