﻿using Newtonsoft.Json.Linq;
using PokeCharts.Exceptions;
using PokeCharts.GraphQl;
using PokeCharts.Models;

namespace PokeCharts.Daos;

public class PokemonDao : IPokemonDao
{
    private readonly GraphQlClient _client;
    private readonly QueryConverter _queryConverter;

    public PokemonDao(IConfiguration configuration)
    {
        _client = new GraphQlClient(configuration);
        _queryConverter = new QueryConverter(configuration);
    }

    public Pokemon Get(int id)
    {
        return SendQuery(ConditionalQuery("id", id.ToString())).FirstOrDefault()
               ?? throw new PokemonNotFoundException(new ModelReference(id));
    }

    public Pokemon Get(string name)
    {
        return SendQuery(ConditionalQuery("name", name)).FirstOrDefault()
               ?? throw new PokemonNotFoundException(new ModelReference(name));
    }

    public List<Pokemon> Get()
    {
        string query = new GraphQlQueryBuilder("")
            .Field("Pokemons: pokemon_v2_pokemon", b => b
                .Field("Id: id")
                .Field("Name: name")
                .Field("Height: height")
                .Field("Weight: weight")
                .Field("Types: pokemon_v2_pokemontypes", b => b
                    .Field("Type: pokemon_v2_type", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                    )
                )
                .Field("Stats: pokemon_v2_pokemonstats", b => b
                    .Field("Stat: pokemon_v2_stat", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                    )
                    .Field("BaseStat: base_stat")
                )
            ).Build();

        return SendQuery(query);
    }
    public List<string> GetNames()
    {
        string query = new GraphQlQueryBuilder("")
          .Field("Pokemons: pokemon_v2_pokemon", b => b
            .Field("Name: name")
          ).Build();
        var result = _client.Execute(query).Result;
        return _queryConverter.ToNamesList(result, "Pokemons");
    }
    
    private List<Pokemon> SendQuery(string query)
    {
        JObject result = _client.Execute(query).Result;
        return _queryConverter.ToPokemons(result);
    }

    private string ConditionalQuery(string field, string value)
    {
        return new GraphQlQueryBuilder("")
            .FieldWithArguments("Pokemons: pokemon_v2_pokemon", b => b
                .Argument("where", b => b
                    .Argument(field, b => b
                        .ArgumentCondition("_eq", value)))
                .EndArguments()
                .Field("Id: id")
                .Field("Name: name")
                .Field("Height: height")
                .Field("Weight: weight")
                .Field("Types: pokemon_v2_pokemontypes", b => b
                    .Field("Type: pokemon_v2_type", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                    )
                )
                .Field("Stats: pokemon_v2_pokemonstats", b => b
                    .Field("Stat: pokemon_v2_stat", b => b
                        .Field("Id: id")
                        .Field("Name: name")
                    )
                    .Field("BaseStat: base_stat")
                )
            ).Build();
    }

    public List<Pokemon> GetFiltered(string types, string stat, string conditions, int? conditionValue)
    {
        //string typeCondition ="[{pokemon_v2_pokemontypes: {pokemon_v2_type: {name: {_eq: 'normal'}}}}, {pokemon_v2_pokemontypes: {pokemon_v2_type: {name: {_eq: 'water'}}}}]";

        /*
        //Get pokemon that contains the one of the types
        string query = new GraphQlQueryBuilder("")
            .Argument("where", b => b
                .ArgumentCondition("_or", typeCondition))
            .EndArguments()
            .Field("Pokemons: pokemon_v2_pokemon", b => b
                .Field("Id: id")
                .Field("Name: name")
            ).Build();*/
            
        string query = "query best_grass_poison_pokemons{pokemon: pokemon_v2_pokemon(where: {_or: [{pokemon_v2_pokemontypes: {pokemon_v2_type: {name: {_eq: \"grass\"}}}}, {pokemon_v2_pokemontypes: {pokemon_v2_type: {name: {_eq: \"poison\"}}}}]}) {name stats: pokemon_v2_pokemonstats_aggregate(order_by: {}) {aggregate {sum {base_stat}}}}}";
        return SendQuery(query);
        
    }
}