using System.ComponentModel;
using System.Net;

namespace PokeCharts.Constants;

public static class RfcLink
{
    public static class Http
    {
        public static class StatusCode
        {
            public enum ClientError
            {
                [Description("https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found")]
                NotFound = HttpStatusCode.NotFound
            }

            public enum ServerError
            {
                [Description("https://datatracker.ietf.org/doc/html/rfc9110#name-501-not-implemented")]
                NotImplemented = HttpStatusCode.NotImplemented
            }

            public static bool TryParseClientError(int errorCode, out ClientError clientError)
            {
                var clientErrorParsed = false;
                clientError = default;
                if (errorCode is >= 400 and < 500)
                    clientErrorParsed = Enum.TryParse(errorCode.ToString(), out clientError);

                return clientErrorParsed;
            }

            public static bool TryParseServerError(int errorCode, out ServerError serverError)
            {
                var serverErrorParsed = false;
                serverError = default;
                if (errorCode is >= 500 and < 600)
                    serverErrorParsed = Enum.TryParse(errorCode.ToString(), out serverError);

                return serverErrorParsed;
            }
        }
    }
}