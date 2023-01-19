namespace PokeCharts.Models;

public record Move(
    int Id,
    string Name,
    int Power,
    Move.Categories Category,
    Type Type)
{
    public enum Categories
    {
        Physical,
        Special,
        Status
    }
}