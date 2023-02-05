using Microsoft.AspNetCore.Mvc;

namespace PokeCharts.Controllers;

[Route(BaseRoute)]
[ApiController]
public class ErrorController : ControllerBase
{
    public const string BaseRoute = "/error";

    [HttpGet]
    public ActionResult<ProblemDetails> HandleError() => Problem();
}