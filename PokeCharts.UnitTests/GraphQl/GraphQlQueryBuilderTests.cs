using PokeCharts.GraphQl;
using PokeCharts.UnitTests.Helpers;

namespace PokeCharts.UnitTests.GraphQl;

[TestFixture]
public class GraphQlQueryBuilderTests
{
    [Test]
    public void Build_SimpleQuery_ReturnsCorrectQuery()
    {
        // Given
        string expectedQuery = ResourceTestHelper.GetGraphQlQuery("SimpleQuery.txt");
        GraphQlQueryBuilder queryBuilder = new GraphQlQueryBuilder("allPokemon")
            .Field("pokemon_v2_pokemon", b => b
                .Field("id")
                .Field("name")
            );

        // When
        string query = queryBuilder.Build();

        // Then
        Assert.That(query, Is.EqualTo(expectedQuery));
    }

    [Test]
    public void Build_QueryWithAliases_ReturnsCoorectQuery()
    {
        // Given
        string expectedQuery = ResourceTestHelper.GetGraphQlQuery("QueryWithAliases.txt");
        GraphQlQueryBuilder queryBuilder = new GraphQlQueryBuilder("allPokemon")
            .Field("Pokemon:pokemon_v2_pokemon", b => b
                .Field("Id:id")
                .Field("Name:name")
                .Field("Types:pokemon_v2_pokemontypes", b => b
                    .Field("Type:pokemon_v2_type", b => b
                        .Field("Id:id")
                        .Field("Name:name")
                    )
                )
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
            .FieldWithArguments("pokemon_v2_type", b => b
                .Argument("where", b => b
                    .Argument("name", b => b
                        .ArgumentCondition("_eq", type)))
                .EndArguments()
                .Field("name")
                .Field("pokemon_v2_pokemontypes", b => b
                    .Field("pokemon_v2_pokemon", b => b
                        .Field("name")
                        .Field("id"))));

        // When
        string query = queryBuilder.Build();

        // Then
        Assert.That(query, Is.EqualTo(expectedQuery));
    }

    [Test]
    public void Build_ComplexQuery_ReturnsCorrectQuery()
    {
        // Given
        string expectedQuery = ResourceTestHelper.GetGraphQlQuery("ComplexQuery.txt");
        GraphQlQueryBuilder queryBuilder = new GraphQlQueryBuilder("weakestPokemonAbleToBeatFireRedAlone")
            .FieldWithArguments("pokemon_v2_pokemon", b => b
                .Argument("where", b => b
                    .Argument("_and", b => b
                        .Argument("pokemon_v2_pokemonspecy", b => b
                            .Argument("pokemon_v2_generation", b => b
                                .Argument("name", b => b
                                    .ArgumentCondition("_eq", "generation-i"))))
                        .Argument("pokemon_v2_pokemonmoves", b => b
                            .Argument("pokemon_v2_move", b => b
                                .Argument("name", b => b
                                    .ArgumentCondition("_eq", "strength"))))
                        .Argument("_and", b => b
                            .Argument("pokemon_v2_pokemonmoves", b => b
                                .Argument("pokemon_v2_move", b => b
                                    .Argument("name", b => b
                                        .ArgumentCondition("_eq", "cut"))))
                            .Argument("_and", b => b
                                .Argument("pokemon_v2_pokemonmoves", b => b
                                    .Argument("pokemon_v2_move", b => b
                                        .Argument("name", b => b
                                            .ArgumentCondition("_eq", "surf"))))))))
                .Argument("order_by", b => b
                    .ArgumentConditionEnum("base_experience", "asc"))
                .ArgumentCondition("limit", 1)
                .EndArguments()
                .Field("pokemon_v2_pokemonspecy", b => b
                    .FieldWithArguments("pokemon_v2_pokemonspeciesnames", b => b
                        .Argument("where", b => b
                            .Argument("pokemon_v2_language", b => b
                                .Argument("name", b => b
                                    .ArgumentCondition("_eq", "en"))))
                        .EndArguments()
                        .Field("name")))
            );

        // When
        string query = queryBuilder.Build();

        // Then
        Assert.That(query, Is.EqualTo(expectedQuery));
    }
}