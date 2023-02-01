using Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;
namespace PokeCharts.Daos;

public class PokemonDao : IPokemonDao
{
    static GraphQLClient _client = new GraphQLClient();
    public Pokemon Get(int id)
    {
        string query = @"query{
              Pokemons: pokemon_v2_pokemon {
                Id: id
                Name:name
                Height:height
                Weight: weight
                Types: pokemon_v2_pokemontypes {
                  Type: pokemon_v2_type {
                    Id: id
                    Name: name
                  }
                }
                Stats: pokemon_v2_pokemonstats {
                  Stat: pokemon_v2_stat {
                    Id: id
                    Name:name
                  }
                  BaseStat:base_stat
                }
                SpriteEndPoint:pokemon_v2_pokemonsprites {
                    Sprites: sprites
                }
              }  
            }";
        var result = _client.Execute(query).Result;
        int i = 1;
        ConvertToPokemons(result);
        throw new NotImplementedException();
    }
    public Pokemon Get(string name)
    {
        throw new NotImplementedException();
    }
    public List<Pokemon> Get()
    {
        throw new NotImplementedException();
    }
    private List<Pokemon> ConvertToPokemons(JObject jsonInput)
    {
        var pokemons = from pokemon in jsonInput?["data"]?["Pokemons"] select pokemon;
        JToken? firstPokemon = pokemons.FirstOrDefault();
        int? id = firstPokemon?["Id"]?.Value<int>();
        List<Pokemon> output = new List<Pokemon>();

        // foreach (var input in dataInput.Pokemons)
        // {
        //     string sprite = GetSprite(input.Sprites);
        //     Stats stats = ConvertToStats(input.Stats);
        //     Type[] types = ConvertToTypes(input.Types);
        //     Pokemon? pokemon = new Pokemon(
        //         input.Id,
        //         input.Name,
        //         input.Height,
        //         input.Weight,
        //         sprite,
        //         stats,
        //         types
        //     );
        //     output.Add(pokemon);
        // }

        throw new NotImplementedException();
    }
    private string GetSprite(JToken jsonInput)
    {
      int i = 1;
        throw new NotImplementedException();
    }
    private Stats ConvertToStats(JToken jsonInput)
    {
      throw new NotImplementedException();
    }
    private Type[] ConvertToTypes(JToken jsonInput)
    {
        throw new NotImplementedException();
    }

}
