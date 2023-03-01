using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokeCharts.Daos;
using PokeCharts.Models;
using System.Xml.Linq;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.Controllers;

[Route("[controller]")]
[ApiController]
public class TypesController : ControllerBase
{
    private readonly ITypeDao _typeDao;

    public TypesController(ITypeDao typeDao)
    {
        _typeDao = typeDao;
    }

    [HttpGet]
    public ActionResult<List<Type>> GetAll() => _typeDao.Get();

    [HttpGet("{id:int}")]
    public ActionResult<Type> Get(int id) => _typeDao.Get(id);

    [HttpGet("{name}")]
    [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
    public ActionResult<Type> Get(string name) => _typeDao.Get(name);
}