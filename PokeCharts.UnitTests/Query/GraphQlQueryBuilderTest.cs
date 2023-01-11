using PokeCharts.Query;
using static PokeCharts.UnitTests.Helpers.ResourceTestHelper;

namespace PokeCharts.UnitTests.Query;

[TestFixture]
public class GraphQlQueryBuilderTest
{
    [Test]
    public void Build_SimpleQuery_ReturnsCorrectQuery()
    {
        // Given
        // Get resource file
        string expectedQuery = GetGraphQlQueryResource("SimpleQuery.txt");
        GraphQlQueryBuilder queryBuilder = new GraphQlQueryBuilder("allPokemon")
            .Field("pokemon_v2_pokemon", builder => builder
                .Field("id")
                .Field("name")
            );

        // When
        string query = queryBuilder.Build();

        // Then
        Assert.That(query, Is.EqualTo(expectedQuery));
    }
}