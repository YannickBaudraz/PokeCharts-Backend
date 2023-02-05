using System.Text.RegularExpressions;

namespace PokeCharts.Constants;

public static partial class Regexps
{
    [GeneratedRegex("(?<!^)(?=[A-Z])")]
    public static partial Regex CamelCaseRegex();
}