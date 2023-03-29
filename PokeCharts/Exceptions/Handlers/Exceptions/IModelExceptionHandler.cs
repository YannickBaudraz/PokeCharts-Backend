using System.Net;

namespace PokeCharts.Exceptions.Handlers.Exceptions;

public interface IModelExceptionHandler
{
    HttpStatusCode? Handle(Exception exception);
}