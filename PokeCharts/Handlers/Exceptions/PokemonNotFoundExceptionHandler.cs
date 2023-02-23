using System.Net;
using PokeCharts.Exceptions;

namespace PokeCharts.Handlers.Exceptions;

public class PokemonNotFoundExceptionHandler : ModelExceptionHandlerBase
{
    protected override Type HandleableException { get; } = typeof(PokemonNotFoundException);
    protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
}