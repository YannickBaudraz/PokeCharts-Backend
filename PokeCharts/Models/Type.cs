namespace PokeCharts.Models;

public class Type
{
    public int id { get; }
    public string name { get; }
    public List<Type>? doubleDamageTo { get; private set; }
    public List<Type>? halfDamageTo { get; private set; }
    public List<Type>? noDamageTo { get; private set; }

    public Type(int id, string name, List<Type> doubleDamageTo = null, List<Type> halfDamageTo = null, List<Type> noDamageTo = null)
    {
        this.id = id;
        this.name = name;
        if (doubleDamageTo != null && halfDamageTo != null && noDamageTo != null)
        {
            AddDamageProperties(doubleDamageTo, halfDamageTo, noDamageTo);
        }
        else
        {
            this.doubleDamageTo = doubleDamageTo;
            this.halfDamageTo = halfDamageTo;
            this.noDamageTo = noDamageTo;
        }
    }
    public void AddDamageProperties(List<Type> doubleDamageTo, List<Type> halfDamageTo, List<Type> noDamageTo)
    {
        this.doubleDamageTo = doubleDamageTo;
        this.halfDamageTo = halfDamageTo;
        this.noDamageTo = noDamageTo;
        List<Type> allLists = this.noDamageTo.Concat(this.halfDamageTo).Concat(this.doubleDamageTo).ToList();
        if (allLists.Count() == allLists.Distinct().Count()) throw new Exception("there are duplicated types");
    }
}
