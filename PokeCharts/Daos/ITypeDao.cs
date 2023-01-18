using PokeCharts.Models;
using Type = PokeCharts.Models.Type;

namespace PokeCharts.Daos;

public interface ITypeDao
{
    Type Get(int id);
    Type Get(string name);
    List<Type> Get();
}
