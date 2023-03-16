using PokeCharts.Models;

namespace PokeCharts.Daos;

public interface IPokemonMoveDao
{
    List<Move> Get(int id);
    List<Move> Get(string name);
}
