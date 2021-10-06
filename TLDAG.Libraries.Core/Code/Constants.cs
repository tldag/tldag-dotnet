using System.Text.RegularExpressions;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.Code
{
    public static partial class CodeConstants

    {
        public const char EndOfLineChar = '\n';
        public const char EndOfFileChar = (char)0xffff;

        public const string EndOfLineName = "EOL";
        public const string EndOfFileName = "EOF";
        public const string ExtendedGrammarRootName = "<root>";

        public static readonly StringSet ReservedTokenNames = new(new string[] { "", EndOfLineName, EndOfFileName });
        public static readonly StringSet ReservedTerminalNames = new(new string[] { EndOfFileName });
        public static readonly StringSet ReservedProductionNames = new(new string[] { ExtendedGrammarRootName });

        public static readonly Regex TokenNameRegex = new("^[A-Z][_A-Z0-9]*$");
        public static readonly Regex ProductionNameRegex = new("^[a-z][-a-z0-9]*$");
    }
}
