using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokeCharts.Models;

public record Pokemon(
    int Id,
    string Name,
    float Height,
    float Weight,
    string Sprite,
    Stats Stats,
    [property: MaxLength(2)] Type[] Types
);