using Newtonsoft.Json.Linq;
using PokeCharts.GraphQl;
using PokeCharts.IntegrationTests.Helpers;

namespace PokeCharts.IntegrationTests.GraphQl;

[TestFixture]
public class GraphQlClientTest
{
    private readonly GraphQlClient _client = new(ConfigTestHelper.Configuration);

    // To prove that we really received data
    private static int? GetPokemonId(JObject jsonOutput)
    {
        var pokemons = from pokemon in jsonOutput["data"]?["pokemon_v2_pokemon"] select pokemon;
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
        Assert.IsNotNull(exception?.Message);
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