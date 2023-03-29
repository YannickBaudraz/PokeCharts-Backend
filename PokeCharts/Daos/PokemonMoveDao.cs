using Newtonsoft.Json.Linq;
using PokeCharts.Exceptions;
using PokeCharts.GraphQl;
using PokeCharts.Models;

namespace PokeCharts.Daos;

public class PokemonMoveDao : IPokemonMoveDao
{
    private readonly GraphQlClient _client;
    private readonly QueryConverter _queryConverter;

    public PokemonMoveDao(IConfiguration configuration)
    {
        _client = new GraphQlClient(configuration);
        _queryConverter = new QueryConverter(configuration);
    }

    public List<Move> Get(int id) => SendQuery(ConditionalQuery("id", id.ToString()), pokemonId: id);

    public List<Move> Get(string name) => SendQuery(ConditionalQuery("name", name), name);

    /// <exception cref="PokemonNotFoundException" />
    private List<Move> SendQuery(string query, string? pokemonName = null, int? pokemonId = null)
    {
        JObject result = _client.Execute(query).Result;
        if (pokemonName != null && result["data"]?["Pokemons"]?.Count() == 0) throw new PokemonNotFoundException(new ModelReference(pokemonName));
        if (pokemonId != null && result["data"]?["Pokemons"]?.Count() == 0) throw new PokemonNotFoundException(new ModelReference(pokemonId));
        return _queryConverter.ToPokemonMoves(result).GroupBy(m => m.Id).Select(g => g.First()).ToList();
    }

    private string ConditionalQuery(string field, string value)
    {
        return new GraphQlQueryBuilder("")
            .FieldWithArguments("Pokemons: pokemon_v2_pokemon", b => b
                .Argument("where", b => b
                    .Argument(field, b => b
                        .ArgumentCondition("_eq", value)))
                .EndArguments()
                .Field("Moves: pokemon_v2_pokemonmoves", b => b
                    .Field("Move: pokemon_v2_move", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                        .Field("Type: pokemon_v2_type", b => b
                            .Field("Id: id")
                            .Field("Name: name")
                        )
                        .Field("Power: power")
                        .Field("DamageClass: pokemon_v2_movedamageclass", b => b
                            .Field("Name: name")
                        )
                    )
                )
            )
            .Build();
    }
}