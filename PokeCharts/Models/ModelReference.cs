namespace PokeCharts.Models;

public class ModelReference
{
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