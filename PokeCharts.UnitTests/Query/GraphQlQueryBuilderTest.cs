using PokeCharts.Query;
using PokeCharts.UnitTests.Helpers;

namespace PokeCharts.UnitTests.Query;

[TestFixture]
public class GraphQlQueryBuilderTest
{
    [Test]
    public void Build_SimpleQuery_ReturnsCorrectQuery()
    {
        // Given
        // Get resource file
        string expectedQuery = ResourceTestHelper.GetGraphQlQuery("SimpleQuery.txt");
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

    [Test]
    public void Build_QueryWithCondition_ReturnsCorrectQuery()
    {
        // Given
        string expectedQuery = ResourceTestHelper.GetGraphQlQuery("QueryWithSimpleCondition.txt");
        const string type = "poison";
        GraphQlQueryBuilder queryBuilder = new GraphQlQueryBuilder("allOfSpecificType")
            .FieldWithArguments("pokemon_v2_type", typeArgBuilder => typeArgBuilder
                .Argument("where", whereBuilder => whereBuilder
                    .Argument("name", nameBuilder => nameBuilder
                        .ArgumentCondition("_eq", type)))
                .EndArguments()
                .Field("name")
                .Field("pokemon_v2_pokemontypes", typeBuilder => typeBuilder
                    .Field("pokemon_v2_pokemon", pokemonBuilder => pokemonBuilder
                        .Field("name")
                        .Field("id"))));

        // When
        string query = queryBuilder.Build();

        // Then
        Assert.That(query, Is.EqualTo(expectedQuery));
    }
}