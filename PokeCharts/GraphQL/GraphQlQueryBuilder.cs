using System.Text;

namespace PokeCharts.GraphQl;

/// <summary>
/// GraphQlQueryBuilder is a class that allows for building GraphQL queries in a fluent and readable way.
/// </summary>
public class GraphQlQueryBuilder
{
    private readonly StringBuilder _queryBuilder = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GraphQlQueryBuilder"/> class.
    /// </summary>
    /// <param name="queryName">Name of the query.</param>
    public GraphQlQueryBuilder(string queryName)
    {
        _queryBuilder.Append($"query {queryName}{{");
    }

    /// <summary>
    /// Adds a field to the query with the given field name.
    /// </summary>
    /// <param name="fieldName">The name of the field.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder Field(string fieldName)
    {
        _queryBuilder.Append($"{fieldName} ");

        return this;
    }

    /// <summary>
    /// Adds a field to the query with the given field name and allows for building a nested sub-query using the subBuilder action.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="subBuilder">The sub builder action.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder Field(string fieldName, Action<GraphQlQueryBuilder> subBuilder)
    {
        _queryBuilder.Append($"{fieldName}{{");
        subBuilder(this);
        _queryBuilder.Append('}');

        return this;
    }

    /// <summary>
    /// Adds a field to the query with the given field name and allows for building a nested sub-query with arguments
    /// using the subBuilder action. Do not forget to call 
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="subBuilder">The sub builder action.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder FieldWithArguments(string fieldName, Action<GraphQlQueryBuilder> subBuilder)
    {
        _queryBuilder.Append($"{fieldName}(");
        subBuilder(this);
        _queryBuilder.Append('}');

        return this;
    }

    /// <summary>
    /// Adds an argument to the current field with the given argument name and allows for building a nested sub-query using the subBuilder action.
    /// </summary>
    /// <param name="argumentName">Name of the argument.</param>
    /// <param name="subBuilder">The sub builder action.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder Argument(string argumentName, Action<GraphQlQueryBuilder> subBuilder)
    {
        _queryBuilder.Append($"{argumentName}:{{");
        subBuilder(this);
        _queryBuilder.Append('}');

        return this;
    }

    /// <summary>
    /// Adds an argument condition to the current field with the given operator name and string value.
    /// </summary>
    /// <param name="operatorName">Name of the operator.</param>
    /// <param name="value">The value of the argument.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder ArgumentCondition(string operatorName, string value)
    {
        _queryBuilder.Append($"{operatorName}:\"{value}\"");

        return this;
    }

    /// <summary>
    /// Adds an argument condition to the current field with the given operator name and integer value.
    /// </summary>
    /// <param name="operatorName">Name of the operator.</param>
    /// <param name="value">The value of the argument.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder ArgumentCondition(string operatorName, int value)
    {
        _queryBuilder.Append($"{operatorName}:{value}");

        return this;
    }

    /// <summary>
    /// Adds an argument condition to the current field with the given operator name and enum value.
    /// </summary>
    /// <param name="operatorName">Name of the operator.</param>
    /// <param name="value">The value of the argument.</param>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder ArgumentConditionEnum(string operatorName, string value)
    {
        _queryBuilder.Append($"{operatorName}:{value}");

        return this;
    }

    /// <summary>
    /// Ends the current argument list and adds closing parenthesis.
    /// </summary>
    /// <returns>The current instance of <see cref="GraphQlQueryBuilder"/> to allow for chaining of method calls.</returns>
    public GraphQlQueryBuilder EndArguments()
    {
        _queryBuilder.Append("){");

        return this;
    }

    /// <summary>
    /// Builds the <see cref="GraphQlQueryBuilder"/> instance into a GraphQL query string.
    /// </summary>
    /// <returns>The query string.</returns>
    public string Build() => _queryBuilder.Append('}').ToString();
}