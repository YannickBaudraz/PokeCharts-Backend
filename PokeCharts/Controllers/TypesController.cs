using Microsoft.AspNetCore.Mvc;
using PokeCharts.Dao;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.Controllers;

[ApiController]
[Route("[controller]")]
public class TypesController : ControllerBase
{
    private readonly ITypeDao _typeDao;

    public TypesController(ITypeDao typeDao)
    {
        _typeDao = typeDao;
    }

    [HttpGet]
    public ActionResult<List<Type>> Get()
    {
        return _typeDao.Get();
    }

    [HttpGet("{id:int}")]
    public ActionResult<Type> Get(int id)
    {
        return _typeDao.Get(id) is { } type
            ? type
            : NotFound();
    }
}