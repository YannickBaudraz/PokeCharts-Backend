namespace PokeCharts.Models;

public record Pokemon(
    int Id,
    string Name,
    float Height,
    float Weight,
    string Sprite,
    Stats Stats
);