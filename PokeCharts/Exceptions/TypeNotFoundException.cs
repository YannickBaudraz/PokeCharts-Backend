using System.Runtime.Serialization;
using PokeCharts.Models;

namespace PokeCharts.Exceptions;

[Serializable]
public class TypeNotFoundException : ModelException
{
    public TypeNotFoundException(ModelReference reference)
        : base($"The Type with the {reference.Parameter} [{reference.Id}] was not found")
    {
    }

    public TypeNotFoundException(string message)
        : base(message)
    {
    }

    public TypeNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected TypeNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}