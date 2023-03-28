using Newtonsoft.Json.Linq;
using PokeCharts.GraphQl;
using PokeCharts.Models;

namespace PokeCharts.Daos;

public class MoveDao : IMoveDao
{
    private readonly GraphQlClient _client;
    private readonly QueryConverter _queryConverter;

    public MoveDao(IConfiguration configuration)
    {
        _client = new GraphQlClient(configuration);
        _queryConverter = new QueryConverter(configuration);
    }

    public Move Get(int id) => SendQuery(ConditionalQuery("id", id.ToString()), moveId: id);

    public Move Get(string name) => SendQuery(ConditionalQuery("name", name), name);

    private Move SendQuery(string query, string? moveName = null, int? moveId = null)
    {
        JObject result = _client.Execute(query).Result;
        return _queryConverter.ToMove(result);
    }

    private string ConditionalQuery(string field, string value)
    {
        return new GraphQlQueryBuilder("")
            .FieldWithArguments("Move: pokemon_v2_move", b => b
                .Argument("where", b => b
                    .Argument(field, b => b
                        .ArgumentCondition("_eq", value)))
                .EndArguments()
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
            .Build();
    }
}