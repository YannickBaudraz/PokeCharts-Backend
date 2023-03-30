using System.ComponentModel;
using System.Net;

namespace PokeCharts.Constants;

/// <summary>
///     Define RFC-related constants.
/// </summary>
public static class Rfc
{
    /// <summary>
    ///     Define HTTP-related constants.
    /// </summary>
    public static class Http
    {
        /// <summary>
        ///     Definition of HTTP status errors
        /// </summary>
        public static class Status
        {
            /// <summary>
            ///     Represents HTTP status errors with their respective RFC links.
            /// </summary>
            public enum Error
            {
                [Description("https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found")]
                NotFound = HttpStatusCode.NotFound,

                [Description("https://datatracker.ietf.org/doc/html/rfc9110#name-501-not-implemented")]
                NotImplemented = HttpStatusCode.NotImplemented
            }

            /// <summary>
            ///     Tries to parse an integer error code into the corresponding Error enum value.
            /// </summary>
            /// <param name="errorCode">The integer error code to parse.</param>
            /// <param name="error">The output Error enum value if the parsing is successful.</param>
            /// <returns>True if the parsing is successful, otherwise False.</returns>
            public static bool TryParseError(int errorCode, out Error error)
            {
                var errorParsed = false;
                error = default;
                if (errorCode is >= 400 and < 600)
                    errorParsed = Enum.TryParse(errorCode.ToString(), out error);

                return errorParsed;
            }
        }
    }
}