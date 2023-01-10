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

        private async Task<string> Post(string query)
        {
            var content = new StringContent(query, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_urlApi, content);
            
            // Throws an exception if the HttpResponseMessage.IsSuccessStatusCode
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<JObject> SubmitQuery(string requestBody)
        {
            var responseString = await Post(requestBody);
            var responseJson = JObject.Parse(responseString);
            
            // Throws an exception if there is an error with the query
            if (responseJson["errors"] != null){
                // TODO write an explicit error in debug mode
                Console.WriteLine(responseJson["errors"]);
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