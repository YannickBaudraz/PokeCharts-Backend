using PokeCharts.Models;
using PokeCharts.Models.Dtos;
using PokeCharts.Requests;

namespace PokeCharts.Daos;

public interface IPokemonDao
{
    Pokemon Get(int id);
    Pokemon Get(string name);
    List<Pokemon> Get();
    List<PokemonLightDto> GetLights();
    List<Pokemon> GetFiltered(PokemonsFilter pokemonsFilter);
    List<float> GetDamage(int attackerId, int targetId, int moveId);
}