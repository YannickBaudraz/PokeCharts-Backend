using PokeCharts.GraphQl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokeCharts.Models;
using Type = PokeCharts.Models.Type;
namespace PokeCharts.Models;

public class QueryConverter {

    private IConfiguration _configuration;

    public QueryConverter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<Pokemon> ToPokemons(JObject jsonInput)
    {
        var pokemons = from pokemon in jsonInput?["data"]?["Pokemons"] select pokemon;
        List<Pokemon> output = new List<Pokemon>();

        foreach (JToken step in pokemons)
        {

            int pokemonId = (int)step["Id"]!;
            string name = (string)step["Name"]!;
            float height = (float)step["Height"]!;
            float weight = (float)step["Weight"]!;
            string urlSuffix = _configuration.GetValue<string>("GraphQl:SpriteSuffix") ?? throw new ArgumentException("The GraphQL sprite suffix is not configured");
            string mainUrl = urlSuffix + "" + pokemonId + ".png";
            string shinyUrl = urlSuffix + "shiny/" + pokemonId + ".png";
            PokemonSprites sprites = new PokemonSprites(mainUrl, shinyUrl);            
            Stats pokemonStats = ToStats(step?["Stats"]!);
            Type[] typeList = ToTypes(step?["Types"]!).ToArray();
            Pokemon pokemon = new Pokemon(pokemonId, name, height, weight, sprites, pokemonStats, typeList);
            output.Add(pokemon);
        }
        
        return output;
    }
    public Stats ToStats(JToken jsonInput)
    {
        int[] statsList = new int[] { 0, 0, 0, 0, 0, 0 };
        foreach (JToken stat in jsonInput)
        {
            int statsId = (int)stat?["Stat"]?["Id"]!;
            string statsName = (string)stat?["Stat"]?["Name"]!;
            int statsBaseStat = (int)stat?["BaseStat"]!;
            switch (statsName)
            {
                case "hp":
                    statsList[0] = statsBaseStat;
                    break;
                case "attack":
                    statsList[1] = statsBaseStat;
                    break;
                case "defense":
                    statsList[2] = statsBaseStat;
                    break;
                case "special-attack":
                    statsList[3] = statsBaseStat;
                    break;
                case "special-defense":
                    statsList[4] = statsBaseStat;
                    break;
                case "speed":
                    statsList[5] = statsBaseStat;
                    break;
            }
        }
        return new Stats(statsList[0], statsList[1], statsList[2], statsList[3], statsList[4], statsList[5]);

    }
    public List<Type> ToTypes(JToken jsonInput)
    {
        List<Type> typeList = new List<Type>();
        foreach (JToken type in jsonInput)
        {
            int typeId = (int)type?["Type"]?["Id"]!;
            string typeName = (string)type?["Type"]?["Name"]!;
            Type type1 = new Type(typeId, typeName);
            typeList.Add(type1);
        }
        return typeList;
    }
}