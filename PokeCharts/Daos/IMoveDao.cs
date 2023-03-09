using PokeCharts.Models;

namespace PokeCharts.Daos;

public interface IMoveDao
{
    Move Get(int id);
    Move Get(string name);
    List<Move> Get();
    List<string> GetNames();
}
