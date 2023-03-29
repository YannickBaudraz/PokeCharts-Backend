using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using PokeCharts.Constants;

namespace PokeCharts.Extensions.Microsoft.AspNetCore.Mvc;

public static class ApiBehaviorOptionsExtensions
{
    /// <summary>
    ///     Configures the <see cref="ApiBehaviorOptions" /> to use the <see cref="Rfc.Http.Status.Error" /> enum
    /// </summary>
    /// <param name="options"></param>
    public static void ConfigureClientErrorMapping(this ApiBehaviorOptions options)
    {
        IEnumerable<int> errorCodes = Enum.GetValues<Rfc.Http.Status.Error>().Cast<int>();

        errorCodes.ToList().ForEach(errorCode =>
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