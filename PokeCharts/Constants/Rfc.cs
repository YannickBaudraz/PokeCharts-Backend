using System.ComponentModel;
using System.Net;

namespace PokeCharts.Constants;

public static class Rfc
{
    public static class Http
    {
        public static class Status
        {
            public enum Error
            {
                [Description("https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found")]
                NotFound = HttpStatusCode.NotFound,

                [Description("https://datatracker.ietf.org/doc/html/rfc9110#name-501-not-implemented")]
                NotImplemented = HttpStatusCode.NotImplemented
            }

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