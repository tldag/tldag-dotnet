using System.Collections;
using System.Collections.Generic;
using static TLDAG.Core.Code.Constants;

namespace TLDAG.Core.Code
{
    public static class Scan
    {
        public class Position
        {
            public readonly int Line;
            public readonly int Column;

            public Position(int line, int column)
            {
                Line = Contract.Arg.Min(line, 1, nameof(line));
                Column = Contract.Arg.Min(column, 1, nameof(column));
            }

            public Position NextColumn => new(Line, Column + 1);
            public Position NextLine => new(Line + 1, 1);

            public static readonly Position Start = new(1, 1);
        }

        public class Token
        {
            public readonly Position Position;
            public readonly string Name;
            public readonly string Value;
            public readonly bool IsEndOfFile;

            public Token(Position position, string name, string value) : this(position, name, value, false) { }

            private Token(Position position, string name, string value, bool isEndOfFile)
            { Position = position; Name = name; Value = value; IsEndOfFile = isEndOfFile; }

            public static Token EndOfFile(Position position) => new(position, EndOfFileName, "", true);
        }
    }

    public class Scanner : IEnumerable<Scan.Token>
    {
        private Internal.Scan.Scanner scanner;

        public Scanner(Rex.IData data, string source) { scanner = new(data, source); }

        public IEnumerator<Scan.Token> GetEnumerator() => scanner.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => scanner.GetEnumerator();
    }
}