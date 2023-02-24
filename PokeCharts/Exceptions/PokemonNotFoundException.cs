using System.Runtime.Serialization;
using PokeCharts.Models;

namespace PokeCharts.Exceptions;

[Serializable]
public class PokemonNotFoundException : ModelException
{
    public PokemonNotFoundException(ModelReference reference)
        : base($"The Pokemon with the {reference.Parameter} [{reference.Id}] was not found")
    {
    }

    public PokemonNotFoundException(string message)
        : base(message)
    {
    }

    public PokemonNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected PokemonNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}