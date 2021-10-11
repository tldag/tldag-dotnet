using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Scan
    {
        internal class Character { }

        internal class Line : IEnumerable<Character>
        {
            public int Number { get; internal set; }
            public bool Last { get; internal set; }

            internal Line(string source) { }

            public IEnumerator<Character> GetEnumerator() => new LineEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => new LineEnumerator();
        }

        internal class LineEnumerator : IEnumerator<Character>
        {
            public Character Current => throw NotYetImplemented();
            object IEnumerator.Current => throw NotYetImplemented();

            public void Dispose() { GC.SuppressFinalize(this); }

            public void Reset()
            {
                throw NotYetImplemented();
            }

            public bool MoveNext()
            {
                throw NotYetImplemented();
            }
        }

        internal class Lines : IEnumerable<Character>
        {
            private readonly Line[] lines;

            private Lines(IEnumerable<Line> source)
            {
                lines = source.ToArray();

                for (int i = 0, n = lines.Length; i < n; ++i) lines[i].Number = i + 1;
                if (lines.Length > 0) lines[lines.Length - 1].Last = true;
            }

            public static Lines Create(string source)
                => new(StringLines.Create(source).Select(line => new Line(line)));

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