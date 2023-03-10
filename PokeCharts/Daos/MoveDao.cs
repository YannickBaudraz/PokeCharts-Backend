﻿using Newtonsoft.Json.Linq;
using PokeCharts.Exceptions;
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

    public Move Get(int id)
    {
        return SendQuery(ConditionalQuery("id", id.ToString())).FirstOrDefault()
               ?? throw new MoveNotFoundException(new ModelReference(id));
    }

    public Move Get(string name)
    {
        return SendQuery(ConditionalQuery("name", name)).FirstOrDefault()
               ?? throw new MoveNotFoundException(new ModelReference(name));
    }

    public List<Move> Get()
    {
        string query = new GraphQlQueryBuilder("")
            .Field("Moves: pokemon_v2_move", b => b
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
        return SendQuery(query);
    }

    private List<Move> SendQuery(string query)
    {
        JObject result = _client.Execute(query).Result;
        return _queryConverter.ToMoves(result);
    }

    private string ConditionalQuery(string field, string value)
    {
        return new GraphQlQueryBuilder("")
            .FieldWithArguments("Moves: pokemon_v2_move", b => b
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
                    .Field("Id: id")
                    .Field("Name: name")
                )
            )
            .Build();
    }
}