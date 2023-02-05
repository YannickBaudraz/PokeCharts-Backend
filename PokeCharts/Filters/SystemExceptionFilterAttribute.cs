using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PokeCharts.Constants;

namespace PokeCharts.Filters;

public class SystemExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly Dictionary<SystemException, (HttpStatusCode StatusCode, string? Rfc7807Type)> _exceptionMap = new()
    {
        // Only very predicable ones are handled here.
        // Update this list with custom exceptions that inherit from SystemException as needed.
        [new NotImplementedException()] = (HttpStatusCode.NotImplemented, RfcLink.Http.StatusCode.ServerError.NotImplemented),
    };

    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is not SystemException systemException) return;

        KeyValuePair<SystemException, (HttpStatusCode StatusCode, string? Rfc7807Type)> exceptionPair =
            _exceptionMap.FirstOrDefault(pair => pair.Key.GetType() == systemException.GetType());

        if (exceptionPair.Value == default) return;

        ProblemDetails problemDetails = exceptionPair.Value.Rfc7807Type is not null
            ? Extensions.ProblemDetails.From(context, exceptionPair.Value.StatusCode, exceptionPair.Value.Rfc7807Type)
            : Extensions.ProblemDetails.From(context, exceptionPair.Value.StatusCode);

        context.Result = new ObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json; charset=utf-8" },
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}