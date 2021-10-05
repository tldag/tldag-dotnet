using System.Text.RegularExpressions;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Code
    {
        public const string EOF = "EOF";
        public const string EOL = "EOL";

        public static readonly StringSet ReservedTokenNames = new(new string[] { "", EOF, EOL });
        public static readonly Regex TokenNameRegex = new("^[A-Z][_A-Z0-9]*$");
    }
}
