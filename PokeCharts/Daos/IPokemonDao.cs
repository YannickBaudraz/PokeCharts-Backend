using PokeCharts.Models;

namespace PokeCharts.Daos;

public interface IPokemonDao
{
    Pokemon Get(int id);
    Pokemon Get(string name);
    List<Pokemon> Get();
    List<string> GetNames();

    List<Pokemon> GetFiltered(string types, string stat, string? conditions, int? conditionValue);
}
