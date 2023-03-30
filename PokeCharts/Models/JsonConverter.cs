using Newtonsoft.Json.Linq;

namespace PokeCharts.Models;

/// <summary>
///     Converts a JSON object to a model.
/// </summary>
public class QueryConverter
{
    public enum Models
    {
        Pokemons,
        Types,
        Stats
    }

    private readonly IConfiguration _configuration;

    public QueryConverter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <exception cref="ArgumentException">Thrown if the GraphQL sprite suffix is not configured.</exception>
    public List<Pokemon> ToPokemons(JObject jsonInput, bool isRoot = true)
    {
        IEnumerable<JToken> pokemons = isRoot
            ? from pokemon in jsonInput?["data"]?["Pokemons"] select pokemon
            : jsonInput;

        var output = new List<Pokemon>();

        foreach (JToken step in pokemons)
        {
            var pokemonId = (int)step["Id"]!;
            var name = (string)step["Name"]!;
            var height = (float)step["Height"]!;
            var weight = (float)step["Weight"]!;
            string urlSuffix = _configuration.GetValue<string>("GraphQl:SpriteSuffix")
                               ?? throw new ArgumentException("The GraphQL sprite suffix is not configured");

            string mainUrl = urlSuffix + "" + pokemonId + ".png";
            string shinyUrl = urlSuffix + "shiny/" + pokemonId + ".png";
            var sprites = new PokemonSprites(mainUrl, shinyUrl);
            Stats pokemonStats = ToStats(step?["Stats"]!, false);
            Type[] typeList = ToTypes(step?["Types"]!, false).ToArray();
            var pokemon = new Pokemon(pokemonId, name, height, weight, sprites, pokemonStats, typeList);
            output.Add(pokemon);
        }

        return output;
    }

    public List<string> ToNamesList(JToken jsonInput, string model)
    {
        IEnumerable<JToken> entities = from entity in jsonInput?["data"]?[model] select entity;
        var nameList = new List<string>();

        foreach (JToken step in entities) nameList.Add((string)step?["Name"]!);

        return nameList;
    }

    public Stats ToStats(JToken jsonInput, bool isRoot = true)
    {
        IEnumerable<JToken> stats = isRoot
            ? from stat in jsonInput?["data"]?["Stats"] select stat
            : jsonInput;

        int[] statsList = { 0, 0, 0, 0, 0, 0 };

        foreach (JToken stat in stats)
        {
            var statsName = (string)stat?["Stat"]?["Name"]!;
            var statsBaseStat = (int)stat?["BaseStat"]!;

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

    private void AddDamagePropertiesToType(Type type, JToken damagePropertiesNode)
    {
        var doubleDamageTo = new List<Type>();
        var halfDamageTo = new List<Type>();
        var noDamageTo = new List<Type>();

        foreach (JToken damageProperty in damagePropertiesNode)
        {
            var factor = (int)damageProperty["Factor"]!;
            var otherTypeId = (int)damageProperty["OtherType"]?["Id"]!;
            var otherTypeName = (string)damageProperty["OtherType"]?["Name"]!;
            var otherType = new Type(otherTypeId, otherTypeName);

            switch (factor)
            {
                case 0:
                    noDamageTo.Add(otherType);
                    break;
                case 50:
                    halfDamageTo.Add(otherType);
                    break;
                case 200:
                    doubleDamageTo.Add(otherType);
                    break;
            }

            type.AddDamageProperties(doubleDamageTo, halfDamageTo, noDamageTo);
        }
    }

    public List<Type> ToTypes(JToken jsonInput, bool isRoot = true)
    {
        IEnumerable<JToken> types = isRoot
            ? from type in jsonInput?["data"]?["Types"] select type
            : jsonInput;

        var typeList = new List<Type>();

        foreach (JToken type in types)
        {
            int typeId = type?["Type"] != null
                ? (int)type["Type"]?["Id"]!
                : (int)type?["Id"]!;

            string typeName = type?["Type"] != null
                ? (string)type?["Type"]?["Name"]!
                : (string)type?["Name"]!;

            var type1 = new Type(typeId, typeName);

            if (type?["DamageProperties"] != null || type?["Type"]?["DamageProperties"] != null)
            {
                JToken damagePropertiesNode = type?["DamageProperties"] ?? type?["Type"]?["DamageProperties"]!;
                AddDamagePropertiesToType(type1, damagePropertiesNode);
            }

            typeList.Add(type1);
        }

        return typeList;
    }

    public List<Move> ToPokemonMoves(JToken jsonInput, bool isRoot = true)
    {
        IEnumerable<JToken> moves = isRoot
            ? from move in jsonInput?["data"]?["Pokemons"]?[0]?["Moves"] select move
            : jsonInput;

        var moveList = new List<Move>();

        foreach (JToken move in moves)
        {
            var moveId = (int)move?["Move"]?["Id"]!;
            var moveName = (string)move?["Move"]?["Name"]!;
            int movePower = move?["Move"]?["Power"]!.Type == JTokenType.Null
                ? 0
                : (int)move?["Move"]?["Power"]!;

            var damageClass = (string)move?["Move"]?["DamageClass"]?["Name"]!;
            damageClass = char.ToUpper(damageClass[0]) + damageClass.Substring(1);
            var category = Enum.Parse<Move.Categories>(damageClass);
            Type moveType = ToMoveType(move?["Move"]?["Type"]!);
            moveList.Add(new Move(moveId, moveName, movePower, category, moveType));
        }

        return moveList;
    }

    public Move ToMove(JToken jsonInput, bool isRoot = true)
    {
        JToken? move = isRoot
            ? jsonInput?["data"]?["Move"]?[0]
            : jsonInput;

        var moveId = (int)move?["Id"]!;
        var moveName = (string)move?["Name"]!;
        int movePower = move?["Power"]!.Type == JTokenType.Null
            ? 0
            : (int)move?["Power"]!;

        var damageClass = (string)move?["DamageClass"]?["Name"]!;
        damageClass = char.ToUpper(damageClass[0]) + damageClass.Substring(1);
        var category = Enum.Parse<Move.Categories>(damageClass);
        Type moveType = ToMoveType(move?["Type"]!);

        return new Move(moveId, moveName, movePower, category, moveType);
    }

    public Type ToMoveType(JToken type)
    {
        var typeId = (int)type?["Id"]!;
        var typeName = (string)type?["Name"]!;
        return new Type(typeId, typeName);
    }
}