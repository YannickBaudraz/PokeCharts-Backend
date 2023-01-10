using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Client.nUnitTests
{
    public class PokeAPIClientTest
    {

        PokeAPIClient _client = new PokeAPIClient();

        [SetUp]
        public void Setup()
        {
            // ..                       
        }

        [Test]
        public void Execute_incorrectQuery_throwException()
        {
            // given
            string query = "Invalid query";

            // when
            var ex = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));
            Assert.That(ex.Message, Is.EqualTo("There was an error executing the GraphQL query."));
        }

        [Test]
        public void Execute_queryWithVariables_caseSuccess()
        {
            // given
            // Refer to SetUp()
            string query = @"
            query GetPokemon($id: Int!) {
                pokemon_v2_pokemon(where: {id: {_eq: $id}}) {
                    name
                    height
                }
            }
            ";
            var variables = new { id = "1" };

            // when
            var result = _client.Execute(query, variables).Result;
            
            // TODO REMOVE the WriteLine
            Console.WriteLine(result);
            
            JArray pokemonArray = (JArray)result["data"]["pokemon_v2_pokemon"];
            JObject firstPokemon = (JObject)pokemonArray[0];
            
            // then
            Assert.AreEqual(firstPokemon["name"].Value<string>(), "bulbasaur");
            Assert.AreEqual(firstPokemon["height"].Value<int>(), 7);
        }

                [Test]
        public void Execute_simpleQuery_caseSuccess()
        {
            // given
            // Refer to SetUp()
            string query = @"query {
                pokemon_v2_pokemon(limit: 3) {
                    name
                    height
                }
            }";

            // when
            var result = _client.Execute(query).Result;
            // TODO REMOVE the WriteLine
            Console.WriteLine(result);

            // then
            Assert.IsNotNull(result);
        }

    }

}