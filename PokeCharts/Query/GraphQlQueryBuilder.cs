using System.Text;

namespace PokeCharts.Query;

public class GraphQlQueryBuilder
{
    private readonly StringBuilder _queryBuilder = new();

    public GraphQlQueryBuilder(string queryName)
    {
        _queryBuilder.Append($"query {queryName}{{");
    }

    public GraphQlQueryBuilder Field(string fieldName)
    {
        _queryBuilder.Append($"{fieldName} ");

        return this;
    }

    public GraphQlQueryBuilder Field(string fieldName, Action<GraphQlQueryBuilder> subBuilder)
    {
        _queryBuilder.Append($"{fieldName}{{");
        subBuilder(this);
        _queryBuilder.Append('}');

        return this;
    }

    public GraphQlQueryBuilder FieldWithArguments(string fieldName, Action<GraphQlQueryBuilder> subBuilder)
    {
        _queryBuilder.Append($"{fieldName}(");
        subBuilder(this);
        _queryBuilder.Append('}');

        return this;
    }

    public GraphQlQueryBuilder Argument(string argumentName, Action<GraphQlQueryBuilder> subBuilder)
    {
        _queryBuilder.Append($"{argumentName}:{{");
        subBuilder(this);
        _queryBuilder.Append('}');

        return this;
    }

    public GraphQlQueryBuilder ArgumentCondition(string operatorName, string value)
    {
        _queryBuilder.Append($"{operatorName}:\"{value}\"");

        return this;
    }

    public GraphQlQueryBuilder ArgumentCondition(string operatorName, int value)
    {
        _queryBuilder.Append($"{operatorName}:{value}");

        return this;
    }

    public GraphQlQueryBuilder ArgumentConditionEnum(string operatorName, string value)
    {
        _queryBuilder.Append($"{operatorName}:{value}");

        return this;
    }

    public GraphQlQueryBuilder EndArguments()
    {
        _queryBuilder.Append("){");

        return this;
    }

    public string Build() => _queryBuilder.Append('}').ToString();
}