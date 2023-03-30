using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PokeCharts.Exceptions.Filters;

public class SystemExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly Dictionary<SystemException, HttpStatusCode> _exceptionMap = new()
    {
        // Only very predicable ones are handled here.
        // Update this list with custom exceptions that inherit from SystemException as needed.
        [new NotImplementedException()] = HttpStatusCode.NotImplemented
    };

    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not SystemException systemException)
            return;

        KeyValuePair<SystemException, HttpStatusCode> exceptionPair =
            _exceptionMap.FirstOrDefault(pair => pair.Key.GetType() == systemException.GetType());

        if (exceptionPair.Value == default)
            return;

        ProblemDetails problemDetails = Extensions.Microsoft.AspNetCore.Mvc.ProblemDetails.From(context, exceptionPair.Value);
        context.Result = new ObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json; charset=utf-8" },
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}