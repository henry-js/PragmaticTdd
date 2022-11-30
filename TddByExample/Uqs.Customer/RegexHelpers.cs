using System.Text.RegularExpressions;

namespace Uqs.Customer;
internal static partial class RegexHelpers
{
    private const string ALPHANUMERIC_UNDERSCORE_REGEX = @"^[a-zA-Z0-9_]+$";
    public static Regex ValidUsername => MyRegex();

    [GeneratedRegex(ALPHANUMERIC_UNDERSCORE_REGEX, RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
