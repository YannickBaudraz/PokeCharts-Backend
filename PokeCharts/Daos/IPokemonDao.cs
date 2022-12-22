using PokeCharts.Models;

namespace PokeCharts.Daos;

public interface IPokemonDao
{
    Pokemon GetPokemon(int id);
    Pokemon GetPokemon(string name);
    List<Pokemon> GetPokemons();
}
