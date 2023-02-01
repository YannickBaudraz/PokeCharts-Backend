using PokeCharts.GraphQl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokeCharts.Models;
using PokeCharts.Exceptions;
using System.Runtime.InteropServices;
using Type = PokeCharts.Models.Type;
using System.Xml.Linq;

namespace PokeCharts.Daos;

public class PokemonDao : IPokemonDao
{
    private GraphQlClient _client;
    private IConfiguration _configuration;
    private QueryConverter _queryConverter;

    public PokemonDao(IConfiguration configuration)
    {
        _client = new GraphQlClient(configuration);
        _configuration = configuration;
        _queryConverter = new QueryConverter(configuration);
    }
    
    public Pokemon Get(int id)
    {
        return SendQuery(ConditionalQuery("id",id.ToString()))[0];
    }
    public Pokemon Get(string name)
    {
        return SendQuery(ConditionalQuery("name",name))[0];
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
    
    private List<Pokemon> SendQuery(string query)
    {
        var result = _client.Execute(query).Result;
        List<Pokemon> output = _queryConverter.ToPokemons(result);
        if (output.Count > 0)
        {
            return output;
        }            
        throw new PokemonNotFoundException();
    }
    

    private string ConditionalQuery(string field,string value)
    {
       return new GraphQlQueryBuilder("")
          .FieldWithArguments("Pokemons: pokemon_v2_pokemon", b => b
            .Argument("where", b => b
                .Argument(field, b => b
                    .ArgumentCondition("_eq",value)))
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
}
