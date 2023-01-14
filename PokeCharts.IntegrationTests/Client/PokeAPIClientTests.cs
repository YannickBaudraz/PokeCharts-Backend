using Newtonsoft.Json.Linq;

namespace Client.nUnitTests
{

    //  This class tests the sending and receiving of requests of the PokeAPI
    public class PokeAPIClientTest
    {

        PokeAPIClient _client = new PokeAPIClient();
        
        // To prove that we really received data
        protected int? GetPokemonId(JObject jsonOutput){
            var pokemons = from pokemon in jsonOutput?["data"]?["pokemon_v2_pokemon"] select pokemon;
            JToken? firstPokemon = pokemons.FirstOrDefault();
            return firstPokemon?["id"]?.Value<int>();
        }

        [Test]
        public void Execute_emptyQuery_throwException()
        {
            // given
            string query = "";

            // when
            var exception = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));

            // Then
            // The returned message will indicate that it's not a valid graphql query
            Assert.IsNotNull(exception?.Message);
        }

        [Test]
        public void Execute_notAQuery_throwException()
        {
            // given
            string query = "Invalid query";

            // when
            var exception = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));

            // Then
            // The returned message will indicate that it's not a valid graphql query
            Assert.IsNotNull(exception?.Message);
        }

        [Test]
        public void Execute_incorrectQuery_throwException()
        {
            // given            /
            string query = @"query {
                non_existent_field(limit: 3, order_by: {id: asc}) {
                    id
                }
            }";

            // when
            var exception = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));

            // Then
            // The returned message will indicate that "non_existent_field" is not found in type: 'query_root'
            Assert.IsNotNull(exception?.Message);
        }

        [Test]
        public void Execute_queryWithVariables_caseSuccess()
        {
            // given
            var variables = new { id = "1" };
            string query = @"
            query GetPokemon($id: Int!) {
                pokemon_v2_pokemon(where: {id: {_eq: $id}}) {
                    id
                }
            }
            ";

            // when
            var result = _client.Execute(query, variables).Result;
           
            // then
            Assert.That(GetPokemonId(result), Is.EqualTo(1));
        }

        [Test]
        public void Execute_simpleQuery_caseSuccess()
        {
            // given
            string query = @"query {
                pokemon_v2_pokemon(limit: 3, order_by: {id: asc}) {
                    id
                }
            }";

            // when
            var result = _client.Execute(query).Result;

            // then
            Assert.That(GetPokemonId(result), Is.EqualTo(1));
        }

    }

}