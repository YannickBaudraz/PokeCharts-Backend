using System.Runtime.Serialization;

namespace PokeCharts.Exceptions;

[Serializable]
public class PokemonNotFoundException : ModelException
{
    public PokemonNotFoundException(int id)
        : base($"The Pokemon with the id [{id}] was not found")
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