using System.Net;

namespace PokeCharts.Models;

public record Pokemon
{
    public int Id { get; }
    public string Name { get; }
    public float Height { get; }
    public float Weight  {get; }
    public string Sprite { get; }
    public Stats? Stats { get; } 
    public Type[]? Types { get; }

    public Pokemon (int id, string name, float height, float weight, string sprite, Stats stats, Type[] types)
    {
        if (types == null || types.Length == 0 || types.Length > 2 ){
            throw new ArgumentException("A pokemon can only have one or two types.");
        }
        this.Id = id;
        this.Name = name;
        this.Height = height;
        this.Weight = weight;
        this.Sprite = sprite;
        this.Stats = stats;
        this.Types = types;
    }
}
