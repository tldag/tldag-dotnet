using System;
using System.Collections;
using System.Collections.Generic;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Scan
    {
        internal class Scanner : IEnumerable<Code.Scan.Token>
        {
            private IEnumerable<Character> source;

            public Scanner(IEnumerable<Character> source) { this.source = source; }
            public Scanner(string source) : this(Lines.Create(source)) { }

            public IEnumerator<Code.Scan.Token> GetEnumerator() => new Enumerator(source);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(source);
        }

        internal class Enumerator : IEnumerator<Code.Scan.Token>
        {
            private readonly IEnumerable<Character> characters;
            private IEnumerator<Character> enumerator;
            private Code.Scan.Position position = Code.Scan.Position.Start;
            private bool done;

            private Code.Scan.Token? current;
            public Code.Scan.Token Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public Enumerator(IEnumerable<Character> characters)
            {
                this.characters = characters;

                enumerator = characters.GetEnumerator();
                done = false;
            }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { enumerator = characters.GetEnumerator(); done = false; }
            public bool MoveNext() => (current = NextToken()) != null;

            private Code.Scan.Token? NextToken()
            {
                if (done) return null;
                if (!enumerator.MoveNext()) { done = true; return Code.Scan.Token.EndOfFile(position); }

                throw NotYetImplemented();
            }
        }
    }
}