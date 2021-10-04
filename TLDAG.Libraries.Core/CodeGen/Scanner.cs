using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TLDAG.Libraries.Core.Collections;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.Core.CodeGen
{
    public class Alphabet : IReadOnlyList<int>
    {
        private readonly int[] map = new int[65536];

        private IntSetOld? classes = null;
        public IntSetOld Classes => classes ??= new(map);

        public Alphabet(IEnumerable<char> symbols)
        {
            FillMap(symbols);
        }

        internal Alphabet() { }

        public int this[int index] => map[index];

        public int Count => Classes.Count;

        private void FillMap(IEnumerable<char> symbols)
        {
            char[] chars = symbols.Distinct().OrderBy(c => c).ToArray();

            for (int i = 0, n = chars.Length, j = 1; i < n; ++i, ++j)
            {
                map[chars[i]] = j;
            }
        }

        public IEnumerator<int> GetEnumerator() => Classes.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Classes.GetEnumerator();

        public void Save(Stream stream)
        {
            IntStreamOld output = new(stream);

            output.Write(map);
        }

        public static Alphabet Load(Stream stream)
        {
            IntStreamOld input = new(stream);
            Alphabet alphabet = new();

            input.Read(alphabet.map);

            return alphabet;
        }
    }

    public class SourcePosition
    {
        public int Line { get; }
        public int Column { get; }

        public SourcePosition(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }

    public class SourceCharacter
    {
        public SourcePosition Position { get; }
        public char Value { get; }

        public SourceCharacter(SourcePosition position, char value)
        {
            Position = position;
            Value = value;
        }
    }

    public class SourceLine : IEnumerable<SourceCharacter>
    {
        private readonly char[] characters;

        public int Line { get; internal set; }
        public bool Last { get; internal set; }
        public string Source { get => new(characters); }

        public SourceLine(int line, bool last, string source)
        {
            Line = line;
            Last = last;
            characters = source.ToCharArray();
        }

        internal SourceLine(string source)
            : this(0, false, source) { }

        public IEnumerator<SourceCharacter> GetEnumerator()
            => new SourceLineEnumerator(Line, Last, characters);

        IEnumerator IEnumerable.GetEnumerator()
            => new SourceLineEnumerator(Line, Last, characters);

        private class SourceLineEnumerator : IEnumerator<SourceCharacter>
        {
            private readonly int line;
            private readonly bool last;
            private readonly char[] characters;
            private int state;

            private SourceCharacter? current = null;
            public SourceCharacter Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public SourceLineEnumerator(int line, bool last, char[] characters)
            {
                this.line = line;
                this.last = last;
                this.characters = characters;
            }

            public void Dispose() { GC.SuppressFinalize(this); }

            public bool MoveNext()
            {
                if (state > characters.Length) return false;
                if (state == characters.Length && last) return false;

                SourcePosition position = new(line, state + 1);

                if (state == characters.Length)
                {
                    current = new(position, '\n');
                }
                else
                {
                    current = new(position, characters[state]);
                }

                ++state;

                return true;
            }

            public void Reset()
            {
                state = 0;
                current = null;
            }
        }
    }

    public class SourceLines : IEnumerable<SourceCharacter>
    {
        private readonly SourceLine[] lines;

        public SourceLines(IEnumerable<SourceLine> lines)
        {
            this.lines = lines.ToArray();

            SetLineNumbers(this.lines);
            SetLast(this.lines);
        }

        private static void SetLineNumbers(SourceLine[] lines)
        {
            for (int i = 0, n = lines.Length; i < n; ++i)
            {
                lines[i].Line = i + 1;
            }
        }

        private static void SetLast(SourceLine[] lines)
        {
            int count = lines.Length;

            if (count > 0)
            {
                lines[count - 1].Last = true;
            }
        }

        public SourceLines(string source)
            : this(new StringLines(source).Select(line => new SourceLine(line))) { }

        public IEnumerator<SourceCharacter> GetEnumerator()
            => new SourceLinesEnumerator(lines);

        IEnumerator IEnumerable.GetEnumerator()
            => new SourceLinesEnumerator(lines);

        private class SourceLinesEnumerator : IEnumerator<SourceCharacter>
        {
            private readonly IEnumerable<SourceLine> lines;
            private IEnumerator<SourceLine> line;
            private IEnumerator<SourceCharacter>? characters;
            private bool done;

            public SourceCharacter Current => characters?.Current ?? throw new InvalidOperationException();
            object IEnumerator.Current => characters?.Current ?? throw new InvalidOperationException();

            public SourceLinesEnumerator(IEnumerable<SourceLine> lines)
            {
                this.lines = lines;
                line = lines.GetEnumerator();
                done = false;
            }

            public void Reset()
            {
                characters = null;
                line = lines.GetEnumerator();
                done = false;
            }

            public bool MoveNext()
            {
                if (done) return false;

                if (characters != null)
                {
                    if (characters.MoveNext()) { return true; }
                    else { characters = null; }
                }

                if (characters == null)
                {
                    if (line.MoveNext())
                    {
                        characters = line.Current.GetEnumerator();
                        return MoveNext();
                    }
                    else
                    {
                        done = true;
                    }
                }

                return false;
            }

            public void Dispose() { GC.SuppressFinalize(this); }
        }
    }

    public class Token
    {
        public SourcePosition Position { get; }
        public string Name { get; }
        public string Value { get; }

        public Token(SourcePosition position, string name, string value)
        {
            Position = position;
            Name = name;
            Value = value;
        }

        public static readonly Token EOF = new(new(0, 0), "EOF", "");
    }

    public class ScannerData
    {
        public Alphabet Alphabet { get; }
        public Transitions Transitions { get; }
        public Accepting Accepting { get; }

        public ScannerData(Alphabet alphabet, Transitions transitions, Accepting accepting)
        {
            Alphabet = alphabet;
            Transitions = transitions;
            Accepting = accepting;
        }

        public void Save(Stream stream)
        {
            Alphabet.Save(stream);
            Transitions.Save(stream);
            Accepting.Save(stream);
        }

        public static ScannerData Load(Stream stream)
        {
            Alphabet alphabet = Alphabet.Load(stream);
            Transitions transitions = Transitions.Load(stream);
            Accepting accepting = Accepting.Load(stream);

            return new(alphabet, transitions, accepting);
        }

        public static ScannerData FromResource(byte[] bytes, bool compressed = true)
        {
            using Stream stream = ResourceUtils.GetStreamFromResource(bytes, compressed);

            return Load(stream);
        }
    }

    public class Scanner : IEnumerable<Token>
    {
        protected Alphabet alphabet;
        protected Transitions transitions;
        protected Accepting accepting;
        protected IEnumerable<SourceCharacter> source;

        public Scanner(ScannerData data, IEnumerable<SourceCharacter> source)
        {
            alphabet = data.Alphabet;
            transitions = data.Transitions;
            accepting = data.Accepting;

            this.source = source;
        }

        public Scanner(ScannerData data, string source)
            : this(data, new SourceLines(source)) { }

        public IEnumerator<Token> GetEnumerator()
            => CreateEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => CreateEnumerator();

        public delegate Token GetNextToken(IEnumerator<SourceCharacter> input);

        protected virtual IEnumerator<Token> CreateEnumerator()
            => new ScannerEnumerator(NextToken, source);

        public virtual Token NextToken(IEnumerator<SourceCharacter> input)
        {
            if (!input.MoveNext()) return Token.EOF;

            int state = 1;
            SourceCharacter current = input.Current;
            SourcePosition start = current.Position;
            StringBuilder value = new();

            while (true)
            {
                char c = current.Value;
                int cc = alphabet[c];

                value.Append(c);
                state = transitions[state, cc];

                if (accepting.Ids.Contains(state))
                {
                    return new(start, accepting[state], value.ToString());
                }

                if (!input.MoveNext()) return Token.EOF;

                current = input.Current;
            }
        }

        public class ScannerEnumerator : IEnumerator<Token>
        {
            protected readonly GetNextToken getNextToken;
            protected IEnumerable<SourceCharacter> source;
            protected IEnumerator<SourceCharacter> input;

            protected Token? current;
            public Token Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public ScannerEnumerator(GetNextToken getNextToken, IEnumerable<SourceCharacter> source)
            {
                this.getNextToken = getNextToken;
                this.source = source;

                input = source.GetEnumerator();
            }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { input = source.GetEnumerator(); }
            public bool MoveNext() => (current = getNextToken(input)) != null;
        }
    }
}
