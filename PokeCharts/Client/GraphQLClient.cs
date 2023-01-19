using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{   

    public class GraphQLClient
    {   
        private static string _api_url = "https://beta.pokeapi.co/graphql/v1beta";
        private readonly HttpClient _httpClient;
        
        public GraphQLClient()
        {
            //Configuration = configuration;
            _httpClient = new HttpClient();

            if (string.IsNullOrWhiteSpace(_api_url))
            {
                throw new ArgumentException("GraphQLAPI is not configured");
            }
        }

        private JObject SerializeResponse(string response)
        {
            var responseJson = JObject.Parse(response);

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
            var response = await _httpClient.PostAsync(_api_url, content);
            
            // Throws an exception if the HttpResponseMessage.IsSuccessStatusCode
            // Refer to SerializeResponse to know if there is an error in the query
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<JObject> SubmitQuery(string requestBody)
        {
            var responseString = await Post(requestBody);
            var responseJson = SerializeResponse(responseString);
            return responseJson;
        }

        public async Task<JObject> Execute(string query)
        {
            var requestBody = JsonConvert.SerializeObject(new { query = query});
            return await SubmitQuery(requestBody);
        }

    }

}