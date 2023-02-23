using System.Net;

namespace PokeCharts.Handlers.Exceptions;

public class MultipleModelExceptionHandlers : IModelExceptionHandler
{
    private readonly List<ModelExceptionHandlerBase> _handlers;

    public MultipleModelExceptionHandlers()
    {
        _handlers = new List<ModelExceptionHandlerBase>
        {
            new PokemonNotFoundExceptionHandler(),
            new TypeNotFoundExceptionHandler()
        };
    }

    public HttpStatusCode? Handle(Exception exception)
    {
        return _handlers
               .Select(handler => handler.Handle(exception))
               .First(code => code is not null);
    }
}