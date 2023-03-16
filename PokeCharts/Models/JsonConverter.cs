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

    public enum Models
    {
        Pokemons,
        Types,
        Stats
    }

    public List<Pokemon> ToPokemons(JObject jsonInput, bool isRoot=true)
    {
        var pokemons = isRoot? from pokemon in jsonInput?["data"]?["Pokemons"] select pokemon: jsonInput;
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
            Stats pokemonStats = ToStats(step?["Stats"]!, false);
            Type[] typeList = ToTypes(step?["Types"]!,false).ToArray();
            Pokemon pokemon = new Pokemon(pokemonId, name, height, weight, sprites, pokemonStats, typeList);
            output.Add(pokemon);
        }
        
        return output;
    }

    public Stats ToStats(JToken jsonInput, bool isRoot=true)
    {
        var stats = isRoot? from stat in jsonInput?["data"]?["Stats"] select stat: jsonInput;
        int[] statsList = new int[] { 0, 0, 0, 0, 0, 0 };
        foreach (JToken stat in stats)
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

    public List<Type> ToTypes(JToken jsonInput, bool isRoot=true)
    {
        var types = isRoot? from type in jsonInput?["data"]?["Types"] select type: jsonInput;
        List<Type> typeList = new List<Type>();
        foreach (JToken type in types)
        {
            int typeId = (int)type?["Type"]?["Id"]!;
            string typeName = (string)type?["Type"]?["Name"]!;
            Type type1 = new Type(typeId, typeName);
            typeList.Add(type1);
        }
        return typeList;
    }

    public List<Move> ToPokemonMoves(JToken jsonInput, bool isRoot=true)
    {
        var moves = isRoot? from move in jsonInput?["data"]?["Pokemons"]?[0]?["Moves"]select move: jsonInput;
        List<Move> moveList = new List<Move>();
        foreach (JToken move in moves)
        {
            int moveId = (int)move?["Move"]?["Id"]!;
            string moveName = (string)move?["Move"]?["Name"]!;
            int movePower = (move?["Move"]?["Power"]!.Type == JTokenType.Null)? 0 : (int)move?["Move"]?["Power"]!;
            string damageClass = (string)move?["Move"]?["DamageClass"]?["Name"]!;
            damageClass = char.ToUpper(damageClass[0]) + damageClass.Substring(1);
            Move.Categories category = Move.Categories.Parse<Move.Categories>(damageClass);
            Type moveType = ToMoveType(move?["Move"]?["Type"]!);
            moveList.Add(new Move(moveId, moveName, movePower, category, moveType));
        }
        return moveList;
    }
    public Type ToMoveType(JToken type)
    {

        int typeId = (int)type?["Id"]!;
        string typeName = (string)type?["Name"]!;
        return new Type(typeId, typeName);
    }

    public List<string> ToNamesList(JToken jsonInput, string model)
    {
        var entities = from entity in jsonInput?["data"]?[model] select entity;
        List<string> nameList = new List<string>();
        foreach (JToken step in entities)
        {
            nameList.Add((string)step?["Name"]!);
        }
        return nameList;
    }
}