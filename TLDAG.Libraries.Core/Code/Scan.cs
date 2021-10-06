using System.Collections.Generic;
using System.Linq;
using static TLDAG.Libraries.Core.Code.CodeConstants;

namespace TLDAG.Libraries.Core.Code
{
    public class ScanPosition
    {
        public readonly int Line;
        public readonly int Column;

        private ScanPosition(int line, int column) { Line = line; Column = column; }

        public ScanPosition NextColumn => new(Line, Column + 1);
        public ScanPosition NextLine => new(Line + 1, 1);

        public static readonly ScanPosition Start = new(1, 1);
    }

    public class ScanCharacter
    {

    }

    public partial class ScanLine
    {
        public int Line { get; internal set; }
        public bool Last { get; internal set; }

        internal ScanLine(string source) { }
    }

    public partial class ScanLines
    {
        private readonly ScanLine[] lines;

        protected ScanLines(IEnumerable<ScanLine> source)
        {
            lines = source.ToArray();

            for (int i = 0, n = lines.Length; i < n; ++i) lines[i].Line = i + 1;
            if (lines.Length > 0) lines[lines.Length - 1].Last = true;
        }

        public static ScanLines Create(string source)
            => new(StringLines.Create(source).Select(line => new ScanLine(line)));
    }

    public class Token
    {
        public readonly ScanPosition Position;
        public readonly string Name;
        public readonly string Value;

        public Token(ScanPosition position, string name, string value) { Position = position; Name = name; Value = value; }

        public static Token EOF(ScanPosition position) => new(position, EndOfFileName, "");
    }

    public partial class Scanner
    {
        public readonly IEnumerable<ScanCharacter> Source;

        public Scanner(RexData rex, IEnumerable<ScanCharacter> source)
        {
            Source = source;
        }

        public Scanner(RexData rex, string source) : this(rex, ScanLines.Create(source)) { }
    }
}
