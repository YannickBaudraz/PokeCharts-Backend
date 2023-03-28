using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Extensions;
using PokeCharts.Constants;

namespace PokeCharts.Extensions.Microsoft.AspNetCore.Mvc;

public static class ProblemDetails
{
    public static global::Microsoft.AspNetCore.Mvc.ProblemDetails From(ExceptionContext context, HttpStatusCode statusCode)
    {
        var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        return problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            (int)statusCode,
            GetTitle(context.Exception),
            detail: context.Exception.Message,
            instance: context.HttpContext.Request.Path
        );
    }

    public static global::Microsoft.AspNetCore.Mvc.ProblemDetails? From(StatusCodeContext statusCodeContext)
    {
        int responseStatusCode = statusCodeContext.HttpContext.Response.StatusCode;
        if (responseStatusCode is < 400 or >= 600)
            return null;

        HttpContext httpContext = statusCodeContext.HttpContext;

        var problemDetailsFactory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        return problemDetailsFactory.CreateProblemDetails(
            httpContext,
            responseStatusCode,
            GetTitle(responseStatusCode),
            instance: httpContext.Request.Path
        );
    }

    private static string GetTitle(int responseStatusCode)
    {
        var statusCode = (HttpStatusCode)responseStatusCode;
        string statusName = statusCode.GetDisplayName();
        return SplitPascalCaseToSpaced(statusName);
    }

    private static string GetTitle(Exception modelException)
    {
        string exceptionNameWithoutSuffix = modelException.GetType().Name[..^"Exception".Length];
        return SplitPascalCaseToSpaced(exceptionNameWithoutSuffix);
    }

    private static string SplitPascalCaseToSpaced(string nameWithoutSpaces)
    {
        string[] words = Regexps.PascalCaseRegex().Split(nameWithoutSpaces);
        return string.Join(" ", words);
    }
}