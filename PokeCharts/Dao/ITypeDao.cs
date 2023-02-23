using Type = PokeCharts.Models.Type;

namespace PokeCharts.Dao;

public interface ITypeDao
{
    Type Get(int id);
    List<Type> Get();
}