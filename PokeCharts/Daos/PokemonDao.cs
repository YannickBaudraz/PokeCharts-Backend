﻿﻿using Newtonsoft.Json.Linq;
using PokeCharts.Exceptions;
using PokeCharts.GraphQl;
using PokeCharts.Models;

namespace PokeCharts.Daos;

public class PokemonDao : IPokemonDao
{
    private readonly GraphQlClient _client;
    private readonly QueryConverter _queryConverter;
    private readonly IMoveDao _moveDao;
    private readonly IPokemonTypeDao _pokemonTypeDao;

    public PokemonDao(IConfiguration configuration, IMoveDao moveDao, IPokemonTypeDao pokemonTypeDao)
    {
        _moveDao = moveDao;
        _pokemonTypeDao = pokemonTypeDao;
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

    public List<Pokemon> GetFiltered(string types, string stat, string? conditions, int? conditionValue)
    {
        List<Pokemon> currentList = new List<Pokemon>();
        List<Pokemon> newList = currentList;
        string currentQuery;

        // Split the types and for each type, get all pokemons of that type
        foreach (string type in types.Split(","))
        {
            currentQuery = new GraphQlQueryBuilder("")
                .FieldWithArguments("Pokemons: pokemon_v2_pokemon", b => b
                    .Argument("where", b => b
                        .Argument("pokemon_v2_pokemontypes", b => b
                            .Argument("pokemon_v2_type", b => b
                                .Argument("name", b => b
                                    .ArgumentCondition("_eq", type)))))
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
            currentList = SendQuery(currentQuery);
            //remove pokemon from currentList that are in newList
            currentList = currentList.Where((p) => !newList.Any((p2) => p2.Id == p.Id)).ToList();
            //add the currentList to the newList
            newList.AddRange(currentList);
            //sort the newList by id
            newList = newList.OrderBy((p) => p.Id).ToList();
        }

        if (conditions != null)
        {
            // Make the currentList equal to the newList
            currentList = newList;

            foreach (string condition in conditions.Split(","))
            {
                // remove all pokemons from the currentList that have a stat greater than the conditionValue
                currentList = currentList.Where((p) =>
                {
                    int statValue = 0;
                    switch (stat)
                    {
                        case "health":
                            statValue = p.Stats!.Hp;
                            break;
                        case "attack":
                            statValue = p.Stats!.Attack;
                            break;
                        case "defense":
                            statValue = p.Stats!.Defense;
                            break;
                        case "specialattack":
                            statValue = p.Stats!.SpecialAttack;
                            break;
                        case "specialdefense":
                            statValue = p.Stats!.SpecialDefense;
                            break;
                        case "speed":
                            statValue = p.Stats!.Speed;
                            break;
                        default:
                            break;
                    }
                    switch (condition)
                    {
                        case ">":
                            return statValue <= conditionValue;
                        case "<":
                            return statValue >= conditionValue;
                        case "=":
                            return statValue != conditionValue;
                        default:
                            return false;
                    }
                }
                ).ToList();

            }
            // remove all pokemons from the newList that are in the currentList
            newList = newList.Except(currentList).ToList();
        }
        return newList;
    }

    public List<float> GetDamage(int attackerId, int targetId, int moveId)
    {
        //transform all ids into their objects
        Pokemon attacker = Get(attackerId);
        Pokemon target = Get(targetId);
        Move move = _moveDao.Get(moveId);
        //get detailed type for move
        PokeCharts.Models.Type moveType = _pokemonTypeDao.Get(move.Type.id);
        //calculate damage multiplier
        float damageMultiplier = 1;
        foreach (PokeCharts.Models.Type type in target.Types!)
        {
            if (moveType.doubleDamageTo!.Contains(type))
            {
                damageMultiplier = damageMultiplier * 2;
            }
            else if (moveType.halfDamageTo!.Contains(type))
            {
                damageMultiplier = damageMultiplier * 0.5f;
            }
            else if (moveType.noDamageTo!.Contains(type))
            {
                damageMultiplier = damageMultiplier * 0;
            }
        }
        float damagedealt = (((((2 * 50) / 5f) + 2) * move.Power * ((float)attacker.Stats!.Attack / (float)target.Stats!.Defense)) / 50f) * damageMultiplier;
        return new List<float> {damagedealt,damageMultiplier};
    }
}