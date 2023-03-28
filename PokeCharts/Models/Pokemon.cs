namespace PokeCharts.Models;

public record Pokemon
{
    public int Id { get; }
    public string Name { get; }
    public float Height { get; }
    public float Weight { get; }
    public PokemonSprites Sprites { get; }
    public Stats? Stats { get; }
    public Type[]? Types { get; }

    public Pokemon(int id, string name, float height, float weight, PokemonSprites sprites, Stats stats, Type[] types)
    {
        if (types == null || types.Length is 0 or > 2)
        {
            throw new ArgumentException("A pokemon can only have one or two types.");
        }

        Id = id;
        Name = name;
        Height = height;
        Weight = weight;
        Sprites = sprites;
        Stats = stats;
        Types = types;
    }
}