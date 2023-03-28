using Type = PokeCharts.Models.Type;

namespace PokeCharts.Daos;

public interface IPokemonTypeDao
{
    Type Get(int id);
    Type Get(string name);
    List<Type> Get();
}