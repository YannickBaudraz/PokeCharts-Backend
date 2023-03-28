namespace PokeCharts.Models;

public class Type
{
    public Type(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public Type(int id, string name, List<Type> doubleDamageTo, List<Type> halfDamageTo, List<Type> noDamageTo)
    {
        this.id = id;
        this.name = name;
        AddDamageProperties(doubleDamageTo, halfDamageTo, noDamageTo);
    }

    public int id { get; }
    public string name { get; }
    public List<Type>? doubleDamageTo { get; private set; }
    public List<Type>? halfDamageTo { get; private set; }
    public List<Type>? noDamageTo { get; private set; }

    public void AddDamageProperties(List<Type> doubleDamageTo, List<Type> halfDamageTo, List<Type> noDamageTo)
    {
        this.doubleDamageTo = doubleDamageTo;
        this.halfDamageTo = halfDamageTo;
        this.noDamageTo = noDamageTo;
        List<Type> allLists = this.noDamageTo.Concat(this.halfDamageTo).Concat(this.doubleDamageTo).ToList();
        if (allLists.Count() != allLists.Distinct().Count()) throw new Exception("there are duplicated types");
    }
}