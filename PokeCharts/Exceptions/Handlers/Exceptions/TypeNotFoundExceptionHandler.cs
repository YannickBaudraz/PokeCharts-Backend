using System.Net;

namespace PokeCharts.Exceptions.Handlers.Exceptions;

public class TypeNotFoundExceptionHandler : ModelExceptionHandlerBase
{
    protected override Type HandleableException { get; } = typeof(TypeNotFoundException);
    protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}