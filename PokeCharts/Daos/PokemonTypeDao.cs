using Newtonsoft.Json.Linq;
using PokeCharts.Exceptions;
using PokeCharts.GraphQl;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.Daos;

public class PokemonTypeDao : IPokemonTypeDao
{
    private readonly GraphQlClient _client;
    private readonly QueryConverter _queryConverter;

    public PokemonTypeDao(IConfiguration configuration)
    {
        _client = new GraphQlClient(configuration);
        _queryConverter = new QueryConverter(configuration);
    }

    public List<Type> Get()
    {
        string query = new GraphQlQueryBuilder("")
            .Field("Types: pokemon_v2_type", b => b
                .Field("Id: id")
                .Field("Name: name")
                .Field("DamageProperties: pokemon_v2_typeefficacies", b => b
                    .Field("Factor: damage_factor")
                    .Field("OtherType: pokemonV2TypeByTargetTypeId", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                    )
                )
            ).Build();

        return SendQuery(query);
    }

    /// <exception cref="TypeNotFoundException"></exception>
    public Type Get(int id) =>
        SendQuery(ConditionalQuery("id", id.ToString())).FirstOrDefault()
        ?? throw new TypeNotFoundException(new ModelReference(id));

    /// <exception cref="TypeNotFoundException"></exception>
    public Type Get(string name) =>
        SendQuery(ConditionalQuery("name", name)).FirstOrDefault()
        ?? throw new TypeNotFoundException(new ModelReference(name));

    private List<Type> SendQuery(string query)
    {
        JObject result = _client.Execute(query).Result;
        return _queryConverter.ToTypes(result);
    }

    private string ConditionalQuery(string field, string value)
    {
        return new GraphQlQueryBuilder("")
            .FieldWithArguments("Types: pokemon_v2_type", b => b
                .Argument("where", b => b
                    .Argument(field, b => b
                        .ArgumentCondition("_eq", value)))
                .EndArguments()
                .Field("Id: id")
                .Field("Name: name")
                .Field("DamageProperties: pokemon_v2_typeefficacies", b => b
                    .Field("Factor: damage_factor")
                    .Field("OtherType: pokemonV2TypeByTargetTypeId", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                    )
                )
            ).Build();
    }
}