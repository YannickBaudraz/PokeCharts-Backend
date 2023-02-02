using PokeCharts.Models;

namespace PokeCharts.Dao;

public interface IPokemonDao
{
    Pokemon Get(int id);
    List<Pokemon> Get();
}