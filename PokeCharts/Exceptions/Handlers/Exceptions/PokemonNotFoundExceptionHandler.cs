using System.Net;

namespace PokeCharts.Exceptions.Handlers.Exceptions;

public class PokemonNotFoundExceptionHandler : ModelExceptionHandlerBase
{
    protected override Type HandleableException { get; } = typeof(PokemonNotFoundException);
    protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}