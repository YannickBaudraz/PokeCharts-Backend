using System.Net;

namespace PokeCharts.Handlers.Exceptions;

public interface IModelExceptionHandler
{
    HttpStatusCode? Handle(Exception exception);
}