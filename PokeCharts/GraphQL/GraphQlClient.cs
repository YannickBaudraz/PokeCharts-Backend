using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PokeCharts.GraphQl;

public class GraphQlClient
{
    private readonly string _apiUrl;
    private readonly HttpClient _httpClient;

    public GraphQlClient(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiUrl = configuration.GetValue<string>("GraphQl:ApiUrl")
                  ?? throw new ArgumentException("The GraphQL API URL is not configured");
    }

    private JObject SerializeResponse(string response)
    {
        JObject? responseJson = JObject.Parse(response);

        if (responseJson.SelectToken("errors") != null)
        {
            var message = responseJson?.SelectToken("errors[0].message")?.Value<string>();
            throw new Exception(message);
        }

        return responseJson;
    }

    private async Task<string> Post(string query)
    {
        var content = new StringContent(query, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(_apiUrl, content);

        // Throws an exception if the HttpResponseMessage.IsSuccessStatusCode
        // Refer to SerializeResponse to know if there is an error in the query
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    private async Task<JObject> SubmitQuery(string requestBody)
    {
        string responseString = await Post(requestBody);
        JObject responseJson = SerializeResponse(responseString);
        return responseJson;
    }

    /// <summary>
    ///     Executes a GraphQL query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<JObject> Execute(string query)
    {
        string requestBody = JsonConvert.SerializeObject(new { query });
        return await SubmitQuery(requestBody);
    }
}