using System.Net;

namespace PokeCharts.Handlers.Exceptions;

public abstract class ModelExceptionHandlerBase : IModelExceptionHandler
{
    protected abstract HttpStatusCode HttpStatusCode { get; }
    protected abstract Type HandleableException { get; }

    public HttpStatusCode? Handle(Exception exception) =>
        HandleableException.IsInstanceOfType(exception)
            ? HttpStatusCode
            : null;
}