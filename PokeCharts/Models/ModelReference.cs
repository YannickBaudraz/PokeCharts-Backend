namespace PokeCharts.Models;

/// <summary>
///     Represents a reference to a model.
/// </summary>
public class ModelReference
{
    /// <summary>
    ///     Creates a new ModelReference.
    /// </summary>
    /// <param name="id">
    ///     The id of the model. Can be a string or an int. If it's a string, the parameter will be "name",
    ///     if it's an int, the parameter will be "id".
    /// </param>
    /// <exception cref="ArgumentException">The id must be a string or an int.</exception>
    public ModelReference(object id)
    {
        Parameter = id switch
        {
            string _ => "name",
            int _ => "id",
            _ => throw new ArgumentException("The id must be a string or an int")
        };

        Id = id;
    }

    public string Parameter { get; }
    public object Id { get; }
}