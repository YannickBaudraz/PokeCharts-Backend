using System.Runtime.Serialization;

namespace PokeCharts.Exceptions;

[Serializable]
public class TypeNotFoundException : ModelException
{
    public TypeNotFoundException(int id)
        : base($"The Type with the id [{id}] was not found")
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