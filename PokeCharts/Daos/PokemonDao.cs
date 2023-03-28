using Newtonsoft.Json.Linq;
using PokeCharts.Exceptions;
using PokeCharts.GraphQl;
using PokeCharts.Models;
using PokeCharts.Models.Dtos;
using PokeCharts.Requests;

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

    public List<PokemonLightDto> GetLights()
    {
        string query = new GraphQlQueryBuilder("")
            .Field("Pokemons: pokemon_v2_pokemon", b => b
                .Field("Name: name")
            ).Build();

        JObject result = _client.Execute(query).Result;
        List<string> names = _queryConverter.ToNamesList(result, "Pokemons");

        return names.Select(name => new PokemonLightDto(name)).ToList();
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

    public List<Pokemon> GetFiltered(PokemonsFilter pokemonsFilter)
    {
        var currentList = new List<Pokemon>();
        List<Pokemon> newList = currentList;
        string currentQuery;

        // Split the types and for each type, get all pokemons of that type
        foreach (string type in pokemonsFilter.Types.Split(","))
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
            newList = newList.Except(currentList).ToList();
            currentList = currentList.Except(newList).ToList();
            newList = newList.Union(currentList).ToList();
        }

        if (pokemonsFilter.Conditions == null) return newList;

        // Make the currentList equal to the newList
        currentList = pokemonsFilter.Conditions
            .Split(",")
            .Aggregate(newList, (current, condition) => current.Where((p) =>
                {
                    int statValue = pokemonsFilter.Stat switch
                    {
                        "health" => p.Stats!.Hp,
                        "attack" => p.Stats!.Attack,
                        "defense" => p.Stats!.Defense,
                        "specialattack" => p.Stats!.SpecialAttack,
                        "specialdefense" => p.Stats!.SpecialDefense,
                        "speed" => p.Stats!.Speed,
                        _ => 0
                    };

                    return condition switch
                    {
                        ">" => statValue <= pokemonsFilter.ConditionValue,
                        "<" => statValue >= pokemonsFilter.ConditionValue,
                        "=" => statValue != pokemonsFilter.ConditionValue,
                        _ => false
                    };
                })
                .ToList());

        return newList.Except(currentList).ToList();
    }
}