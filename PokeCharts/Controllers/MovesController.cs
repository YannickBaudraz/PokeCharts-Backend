using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeCharts.Daos;
using PokeCharts.Models;
using System.Xml.Linq;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.Controllers;

[Route("[controller]")]
[ApiController]
public class MovesController : ControllerBase
{
    private readonly IMoveDao _moveDao;

    public MovesController(IMoveDao moveDao)
    {
        _moveDao = moveDao;
    }

    [HttpGet]
    public ActionResult<List<Move>> GetAll() => _moveDao.Get();

    [HttpGet("{id:int}")]
    public ActionResult<Move> Get(int id) => _moveDao.Get(id);

    [HttpGet("{name}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<Move> Get(string name) => _moveDao.Get(name);
}