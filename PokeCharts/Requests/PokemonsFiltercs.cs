namespace PokeCharts.Requests;

public class PokemonsFilter
{
    public string? Types { get; set; }

    public string? Stat { get; set; }

    public string? Conditions { get; set; }

    public int? ConditionValue { get; set; }

    public bool IsNotEmpty() => Types is not null || Stat is not null || Conditions is not null || ConditionValue is not null;
}