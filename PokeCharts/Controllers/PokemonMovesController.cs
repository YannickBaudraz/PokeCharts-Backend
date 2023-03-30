using Microsoft.AspNetCore.Mvc;
using PokeCharts.Daos;
using PokeCharts.Models;

namespace PokeCharts.Controllers;

[Route("[controller]")]
[ApiController]
public class PokemonMovesController : ControllerBase
{
    private readonly IPokemonMoveDao _moveDao;

    public PokemonMovesController(IPokemonMoveDao moveDao)
    {
        _moveDao = moveDao;
    }

    [HttpGet("{pokemonId:int}")]
    public ActionResult<List<Move>> GetAll(int pokemonId) => _moveDao.Get(pokemonId);

    [HttpGet("{pokemonName}")]
    public ActionResult<List<Move>> GetAll(string pokemonName) => _moveDao.Get(pokemonName);
}