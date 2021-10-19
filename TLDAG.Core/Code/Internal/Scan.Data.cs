using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Code.Scan;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Scan
    {
        internal class Character
        {
            public Position Position { get; }
            public char Value { get; }

            public Character(Position position, char value)
            { Position = position; Value = value; }
        }

        internal class Line : IEnumerable<Character>
        {
            public int Number { get; }
            public bool Last { get; }

            private readonly char[] characters;

            internal Line(int number, bool last, string source)
            { Number = number; Last = last; characters = source.ToCharArray(); }

            public IEnumerator<Character> GetEnumerator() => new LineEnumerator(Number, Last, characters);
            IEnumerator IEnumerable.GetEnumerator() => new LineEnumerator(Number, Last, characters);
        }

        internal class LineEnumerator : IEnumerator<Character>
        {
            private Character? current = null;
            public Character Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            private readonly int line;
            private readonly bool last;
            private readonly char[] characters;
            private int index = 0;

            public LineEnumerator(int line, bool last, char[] characters)
            {
                this.line = line;
                this.last = last;
                this.characters = characters;
            }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { index = 0; current = null; }

            public bool MoveNext()
            {
                if (index > characters.Length) return false;
                if (index == characters.Length && last) return false;

                Position position = new(line, index + 1);

                if (index == characters.Length) { current = new(position, '\n'); }
                else { current = new(position, characters[index]); }

                ++index; return true;
            }
        }

        internal class Lines : IEnumerable<Character>
        {
            private readonly Line[] lines;

            private Lines(IEnumerable<Line> source)
            {
                lines = source.ToArray();
            }

            public static Lines Create(string source) => throw NotYetImplemented();

            public IEnumerator<Character> GetEnumerator() => new LinesEnumerator(lines);
            IEnumerator IEnumerable.GetEnumerator() => new LinesEnumerator(lines);
        }

        public class LinesEnumerator : IEnumerator<Character>
        {
            private readonly IEnumerable<Line> lines;
            private IEnumerator<Line> line;
            private IEnumerator<Character>? character = null;
            private bool done = false;

            private Character? current = null;
            public Character Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public LinesEnumerator(IEnumerable<Line> lines) { this.lines = lines; line = lines.GetEnumerator(); }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { line = lines.GetEnumerator(); character = null; current = null; done = false; }
            public bool MoveNext() => (current = NextCharacter()) != null;

            private Character? NextCharacter()
            {
                if (done) return null;

                if (character != null)
                {
                    if (character.MoveNext()) return character.Current;
                    else character = null;
                }

                if (line.MoveNext()) { character = line.Current.GetEnumerator(); return NextCharacter(); }

                done = true; return null;
            }
        }
    }
}