using System.Net;

namespace PokeCharts.Exceptions.Handlers.Exceptions;

public interface IModelExceptionHandler
{
    /// <summary>
    ///     Handles the exception and returns the appropriate HTTP status code.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    HttpStatusCode? Handle(Exception exception);
}