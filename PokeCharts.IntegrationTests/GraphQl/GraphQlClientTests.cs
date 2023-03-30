using Newtonsoft.Json.Linq;
using PokeCharts.GraphQl;
using PokeCharts.IntegrationTests.Helpers;

namespace PokeCharts.IntegrationTests.GraphQl;

[TestFixture]
public class GraphQlClientTest
{
    private readonly GraphQlClient _client = new(ConfigTestHelper.Configuration);

    // Checks that the returned data are those expected
    private static int? GetPokemonId(JObject jsonOutput)
    {
        IEnumerable<JToken> pokemons = from pokemon in jsonOutput["data"]?["pokemon_v2_pokemon"] select pokemon;
        JToken? firstPokemon = pokemons.FirstOrDefault();
        return firstPokemon?["id"]?.Value<int>();
    }

    [Test]
    public void Execute_CorrectQuerySyntax_Success()
    {
        // given
        const string query = @"query {
                pokemon_v2_pokemon(limit: 3, order_by: {id: asc}) {
                    id
                }
            }";

        // when
        JObject result = _client.Execute(query).Result;

        // then
        Assert.That(GetPokemonId(result), Is.EqualTo(1));
    }

    [Test]
    public void Execute_NullQuery_ThrowsException()
    {
        // given
        const string query = "";

        // when
        var exception = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));

        // Then
        Assert.That(exception?.Message, Is.Not.Null);
    }

    [Test]
    public void Execute_IncorrectQuerySyntax_ThrowsException()
    {
        // given
        const string query = "Invalid query";

        // when
        var exception = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));

        // Then
        Assert.That(exception?.Message, Is.Not.Null);
    }

    [Test]
    public void Execute_QueryContainsNonExistentField_ThrowsException()
    {
        // given            
        const string query = @"query {
                non_existent_field(limit: 3, order_by: {id: asc}) {
                    id
                }
            }";

        // when
        var exception = Assert.ThrowsAsync<Exception>(() => _client.Execute(query));

        // Then
        Assert.That(exception?.Message, Is.Not.Null);
    }
}