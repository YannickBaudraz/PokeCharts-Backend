using System.Net;
using PokeCharts.Exceptions;

namespace PokeCharts.Handlers.Exceptions;

public class TypeNotFoundExceptionHandler : ModelExceptionHandlerBase
{
    protected override Type HandleableException { get; } = typeof(TypeNotFoundException);
    protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}