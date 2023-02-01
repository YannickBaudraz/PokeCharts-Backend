using Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokeCharts.Models;
using System.Runtime.InteropServices;
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
        List<Pokemon> output = new List<Pokemon>();

        foreach (JToken step in pokemons)
        {

            int pokemonId = (int)step?["Id"]!;
            string Name = (string)step?["Name"]!;
            float Height = (float)step?["Height"]!;
            float Weight = (float)step?["Weight"]!;
            string SpriteEndPoint = (string)step?["SpriteEndPoint"]?["Sprites"]!;
            var types = from type in step?["Types"] select type;
            List<Type> typeList = new List<Type>();
            foreach (JToken type in types){
                int typeId = (int)type?["Type"]?["Id"]!;
                string typeName = (string)type?["Type"]?["Name"]!;
                Type type1 = new Type(typeId, typeName);
                typeList.Add(type1);
            }
            var stats = from stat in step?["Stats"] select stat;
            //int [] statsList = new int[] { 0, 0, 0, 0, 0, 0 };
            //foreach (JToken stat in stats)
            //{
            //    int statsId = (int)stat?["Stat"]?["Id"]!;
            //    string statsName = (string)stat?["Stat"]?["Name"]!;
            //}
            //Pokemon pokemon = new Pokemon(pokemonId, Name, Height, Weight, SpriteEndPoint, statsList, typeList.ToArray());
            //output.Add(pokemon);
        }
        
        return output;
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
