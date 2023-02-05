using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PokeCharts.Constants;

namespace PokeCharts.Extensions;

public static class ProblemDetails
{
    public static Microsoft.AspNetCore.Mvc.ProblemDetails From(ExceptionContext context, HttpStatusCode statusCode)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        return problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            statusCode: (int)statusCode,
            title: GetTitle(context.Exception),
            detail: context.Exception.Message,
            instance: context.HttpContext.Request.Path
        );
    }

    private static string GetTitle(Exception modelException)
    {
        string exceptionNameWithoutSuffix = modelException.GetType().Name[..^"Exception".Length];
        string[] words = Regexps.CamelCaseRegex().Split(exceptionNameWithoutSuffix);
        return string.Join(" ", words);
    }
}