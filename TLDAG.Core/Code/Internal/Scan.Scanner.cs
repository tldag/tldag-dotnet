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
            internal Lines Source;

            public IEnumerator<Code.Scan.Token> GetEnumerator() => new Enumerator(this);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);
        }

        internal class Enumerator : IEnumerator<Code.Scan.Token>
        {
            private readonly Scanner scanner;
            private IEnumerator<Character> input;
            private Code.Scan.Position position = Code.Scan.Position.Start;
            private bool done;

            private Code.Scan.Token? current;
            public Code.Scan.Token Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public Enumerator(Scanner scanner)
            {
                this.scanner = scanner;

                input = scanner.Source.GetEnumerator();
                done = false;
            }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { input = scanner.Source.GetEnumerator(); done = false; }
            public bool MoveNext() => (current = NextToken()) != null;

            private Code.Scan.Token? NextToken()
            {
                if (done) return null;
                if (!input.MoveNext()) { done = true; return Code.Scan.Token.EndOfFile(position); }

                throw NotYetImplemented();
            }
        }
    }
}