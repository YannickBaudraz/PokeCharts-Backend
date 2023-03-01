using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PokeCharts.Extensions.Microsoft.AspNetCore.Diagnostics;

public static class StatusCodeContextExtensions
{
    public static async Task WriteProblemDetailsAsJsonAsync(this StatusCodeContext statusCodeContext)
    {
        int responseStatusCode = statusCodeContext.HttpContext.Response.StatusCode;
        if (responseStatusCode is < 400 or >= 600) return;

        ProblemDetails? problemDetails = Mvc.ProblemDetails.From(statusCodeContext);
        if (problemDetails is null) return;

        statusCodeContext.HttpContext.Response.ContentType = "application/problem+json; charset=utf-8";
        await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}