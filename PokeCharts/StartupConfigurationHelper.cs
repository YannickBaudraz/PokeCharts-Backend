using System.ComponentModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using PokeCharts.Constants;

namespace PokeCharts;

public static class StartupConfigurationHelper
{
    public static async Task WriteProblemDetailsAsJsonAsync(StatusCodeContext statusCodeContext)
    {
        int responseStatusCode = statusCodeContext.HttpContext.Response.StatusCode;
        if (responseStatusCode is < 400 or >= 600)
            return;

        ProblemDetails? problemDetails = Extensions.ProblemDetails.From(statusCodeContext);
        if (problemDetails is null)
            return;

        statusCodeContext.HttpContext.Response.ContentType = "application/problem+json; charset=utf-8";
        await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(problemDetails);
    }

    public static void ConfigureClientErrorMapping(ApiBehaviorOptions options)
    {
        IEnumerable<int> errorCodes = Enum.GetValues<Rfc.Http.Status.Error>().Cast<int>();

        errorCodes.ToList()
                  .ForEach(errorCode =>
                  {
                      EnsureErrorMappingExists(options, errorCode);
                      AddErrorMappingData(options, errorCode);
                  });
    }

    private static void EnsureErrorMappingExists(ApiBehaviorOptions options, int errorCode)
    {
        bool shouldAddNewMapping = options.ClientErrorMapping.TryGetValue(errorCode, out _) is false;
        if (shouldAddNewMapping)
            options.ClientErrorMapping.Add(errorCode, new ClientErrorData());
    }

    private static void AddErrorMappingData(ApiBehaviorOptions options, int errorCode)
    {
        bool errorParsed = Rfc.Http.Status.TryParseError(errorCode, out Rfc.Http.Status.Error error);
        if (errorParsed is false)
            return;

        string? description = error.GetAttributeOfType<DescriptionAttribute>()?.Description;
        options.ClientErrorMapping[errorCode].Link = description;
        options.ClientErrorMapping[errorCode].Title = error.GetDisplayName();
    }
}