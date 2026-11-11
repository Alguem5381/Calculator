using System.Globalization;

using Calculator.Core;
using Calculator.Core.Nodes;

using Sprache;

namespace Calculator.Console.Parser;

public class MinimalParser
{
    public static readonly Parser<string> CustomDecimal =
        from integral in Parse.Number
        from fraction in Parse.Char('.')
            .Then(c => Parse.Number.Select(n => c + n))
            .Optional()
        select integral + fraction.GetOrElse("");

    public static readonly Parser<Node> Number =
        from number in CustomDecimal.Token()
        select new Number(double.Parse(number, CultureInfo.InvariantCulture));
}