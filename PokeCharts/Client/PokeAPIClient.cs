using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    public class PokeAPIClient
    {
        private readonly HttpClient _httpClient;
        private string _urlApi = "https://beta.pokeapi.co/graphql/v1beta";

        public PokeAPIClient()
        {
            _httpClient = new HttpClient();
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
            var response = await _httpClient.PostAsync(_urlApi, content);
            
            // Throws an exception if the HttpResponseMessage.IsSuccessStatusCode
            // Refer to SerializeResponse to know if there is an error in the query
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<JObject> SubmitQuery(string requestBody)
        {
            var responseString = await Post(requestBody);
            var responseJson = SerializeResponse(responseString);
            
            // Throws an exception if there is an error with the query
            if (responseJson["errors"] != null){
                throw new Exception("There was an error executing the GraphQL query.");
            }

            return responseJson;
        }

        public async Task<JObject> Execute(string query)
        {
            var requestBody = JsonConvert.SerializeObject(new { query = query});
            return await SubmitQuery(requestBody);
        }

        public async Task<JObject> Execute(string query, object variables)
        {
            var requestBody = JsonConvert.SerializeObject(new { query = query, variables = variables });
            return await SubmitQuery(requestBody);
        }

    }

}