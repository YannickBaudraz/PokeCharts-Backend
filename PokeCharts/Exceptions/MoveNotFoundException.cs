using System.Runtime.Serialization;
using PokeCharts.Models;

namespace PokeCharts.Exceptions;

[Serializable]
public class MoveNotFoundException : ModelException
{
    public MoveNotFoundException(ModelReference reference)
        : base($"The Move with the {reference.Parameter} [{reference.Id}] was not found")
    {
    }

    public MoveNotFoundException(string message)
        : base(message)
    {
    }

    public MoveNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected MoveNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}