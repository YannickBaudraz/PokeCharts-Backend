using System.Runtime.Serialization;

namespace PokeCharts.Exceptions;

/// <summary>
///     The base class for all model exceptions.
/// </summary>
[Serializable]
public abstract class ModelException : Exception
{
    protected ModelException()
    {
    }

    protected ModelException(string message)
        : base(message)
    {
    }

    protected ModelException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ModelException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}