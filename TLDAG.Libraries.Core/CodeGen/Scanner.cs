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
    public class AlphabetOld : IReadOnlyList<int>
    {
        private readonly int[] map = new int[65536];

        private IntSetOld? classes = null;
        public IntSetOld Classes => classes ??= new(map);

        public AlphabetOld(IEnumerable<char> symbols)
        {
            FillMap(symbols);
        }

        internal AlphabetOld() { }

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

        public static AlphabetOld Load(Stream stream)
        {
            IntStreamOld input = new(stream);
            AlphabetOld alphabet = new();

            input.Read(alphabet.map);

            return alphabet;
        }
    }

    public class SourcePositionOld
    {
        public int Line { get; }
        public int Column { get; }

        public SourcePositionOld(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }

    public class SourceCharacterOld
    {
        public SourcePositionOld Position { get; }
        public char Value { get; }

        public SourceCharacterOld(SourcePositionOld position, char value)
        {
            Position = position;
            Value = value;
        }
    }

    public class SourceLineOld : IEnumerable<SourceCharacterOld>
    {
        private readonly char[] characters;

        public int Line { get; internal set; }
        public bool Last { get; internal set; }
        public string Source { get => new(characters); }

        public SourceLineOld(int line, bool last, string source)
        {
            Line = line;
            Last = last;
            characters = source.ToCharArray();
        }

        internal SourceLineOld(string source)
            : this(0, false, source) { }

        public IEnumerator<SourceCharacterOld> GetEnumerator()
            => new SourceLineEnumerator(Line, Last, characters);

        IEnumerator IEnumerable.GetEnumerator()
            => new SourceLineEnumerator(Line, Last, characters);

        private class SourceLineEnumerator : IEnumerator<SourceCharacterOld>
        {
            private readonly int line;
            private readonly bool last;
            private readonly char[] characters;
            private int state;

            private SourceCharacterOld? current = null;
            public SourceCharacterOld Current => current ?? throw new InvalidOperationException();
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

                SourcePositionOld position = new(line, state + 1);

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

    public class SourceLinesOld : IEnumerable<SourceCharacterOld>
    {
        private readonly SourceLineOld[] lines;

        public SourceLinesOld(IEnumerable<SourceLineOld> lines)
        {
            this.lines = lines.ToArray();

            SetLineNumbers(this.lines);
            SetLast(this.lines);
        }

        private static void SetLineNumbers(SourceLineOld[] lines)
        {
            for (int i = 0, n = lines.Length; i < n; ++i)
            {
                lines[i].Line = i + 1;
            }
        }

        private static void SetLast(SourceLineOld[] lines)
        {
            int count = lines.Length;

            if (count > 0)
            {
                lines[count - 1].Last = true;
            }
        }

        public SourceLinesOld(string source)
            : this(new StringLines(source).Select(line => new SourceLineOld(line))) { }

        public IEnumerator<SourceCharacterOld> GetEnumerator()
            => new SourceLinesEnumerator(lines);

        IEnumerator IEnumerable.GetEnumerator()
            => new SourceLinesEnumerator(lines);

        private class SourceLinesEnumerator : IEnumerator<SourceCharacterOld>
        {
            private readonly IEnumerable<SourceLineOld> lines;
            private IEnumerator<SourceLineOld> line;
            private IEnumerator<SourceCharacterOld>? characters;
            private bool done;

            public SourceCharacterOld Current => characters?.Current ?? throw new InvalidOperationException();
            object IEnumerator.Current => characters?.Current ?? throw new InvalidOperationException();

            public SourceLinesEnumerator(IEnumerable<SourceLineOld> lines)
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

    public class TokenOld
    {
        public SourcePositionOld Position { get; }
        public string Name { get; }
        public string Value { get; }

        public TokenOld(SourcePositionOld position, string name, string value)
        {
            Position = position;
            Name = name;
            Value = value;
        }

        public static readonly TokenOld EOF = new(new(0, 0), "EOF", "");
    }

    public class ScannerDataOld
    {
        public AlphabetOld Alphabet { get; }
        public TransitionsOld Transitions { get; }
        public AcceptingOld Accepting { get; }

        public ScannerDataOld(AlphabetOld alphabet, TransitionsOld transitions, AcceptingOld accepting)
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

        public static ScannerDataOld Load(Stream stream)
        {
            AlphabetOld alphabet = AlphabetOld.Load(stream);
            TransitionsOld transitions = TransitionsOld.Load(stream);
            AcceptingOld accepting = AcceptingOld.Load(stream);

            return new(alphabet, transitions, accepting);
        }

        public static ScannerDataOld FromResource(byte[] bytes, bool compressed = true)
        {
            using Stream stream = ByteIO.ToStream(bytes, compressed);

            return Load(stream);
        }
    }

    public class ScannerOld
        : IEnumerable<TokenOld>
    {
        protected AlphabetOld alphabet;
        protected TransitionsOld transitions;
        protected AcceptingOld accepting;
        protected IEnumerable<SourceCharacterOld> source;

        public ScannerOld(ScannerDataOld data, IEnumerable<SourceCharacterOld> source)
        {
            alphabet = data.Alphabet;
            transitions = data.Transitions;
            accepting = data.Accepting;

            this.source = source;
        }

        public ScannerOld(ScannerDataOld data, string source)
            : this(data, new SourceLinesOld(source)) { }

        public IEnumerator<TokenOld> GetEnumerator()
            => CreateEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => CreateEnumerator();

        public delegate TokenOld GetNextToken(IEnumerator<SourceCharacterOld> input);

        protected virtual IEnumerator<TokenOld> CreateEnumerator()
            => new ScannerEnumeratorOld(NextToken, source);

        public virtual TokenOld NextToken(IEnumerator<SourceCharacterOld> input)
        {
            if (!input.MoveNext()) return TokenOld.EOF;

            int state = 1;
            SourceCharacterOld current = input.Current;
            SourcePositionOld start = current.Position;
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

                if (!input.MoveNext()) return TokenOld.EOF;

                current = input.Current;
            }
        }

        public class ScannerEnumeratorOld : IEnumerator<TokenOld>
        {
            protected readonly GetNextToken getNextToken;
            protected IEnumerable<SourceCharacterOld> source;
            protected IEnumerator<SourceCharacterOld> input;

            protected TokenOld? current;
            public TokenOld Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public ScannerEnumeratorOld(GetNextToken getNextToken, IEnumerable<SourceCharacterOld> source)
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
