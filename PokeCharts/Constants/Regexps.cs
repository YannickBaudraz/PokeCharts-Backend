using System.Text.RegularExpressions;

namespace PokeCharts.Constants;

public static partial class Regexps
{
    /// <remarks>
    ///     Pattern explanation:<br />
    ///     <code>
    /// ○ Zero-width negative lookbehind.<br />
    ///     ○ Match if at the beginning of the string.<br />
    /// ○ Zero-width positive lookahead.<br />
    ///     ○ Match a character in the set [A-Z].<br />
    /// </code>
    /// </remarks>
    [GeneratedRegex("(?<!^)(?=[A-Z])")]
    public static partial Regex PascalCaseRegex();
}