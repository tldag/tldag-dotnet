using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.Libraries.Core.CodeGen
{
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
}