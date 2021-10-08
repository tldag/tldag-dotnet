using System;
using System.Collections;
using System.Collections.Generic;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public partial class ScanLine : IEnumerable<ScanCharacter>
    {
        public IEnumerator<ScanCharacter> GetEnumerator() => new ScanLineEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => new ScanLineEnumerator();
    }

    public class ScanLineEnumerator : IEnumerator<ScanCharacter>
    {
        public ScanCharacter Current => throw NotYetImplemented();
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

    public partial class ScanLines : IEnumerable<ScanCharacter>
    {
        public IEnumerator<ScanCharacter> GetEnumerator() => new ScanLinesEnumerator(lines);
        IEnumerator IEnumerable.GetEnumerator() => new ScanLinesEnumerator(lines);
    }

    public class ScanLinesEnumerator : IEnumerator<ScanCharacter>
    {
        protected readonly IEnumerable<ScanLine> lines;
        protected IEnumerator<ScanLine> line;
        protected IEnumerator<ScanCharacter>? character = null;
        protected bool done = false;

        protected ScanCharacter? current = null;
        public ScanCharacter Current => current ?? throw new InvalidOperationException();
        object IEnumerator.Current => current ?? throw new InvalidOperationException();

        public ScanLinesEnumerator(IEnumerable<ScanLine> lines) { this.lines = lines; line = lines.GetEnumerator(); }

        public void Dispose() { GC.SuppressFinalize(this); }
        public void Reset() { line = lines.GetEnumerator(); character = null; current = null; done = false; }
        public bool MoveNext() => (current = NextCharacter()) != null;

        private ScanCharacter? NextCharacter()
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

    public partial class Scanner : IEnumerable<Token>
    {
        public IEnumerator<Token> GetEnumerator() => new ScannerEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => new ScannerEnumerator(this);
    }

    public class ScannerEnumerator : IEnumerator<Token>
    {
        protected readonly Scanner scanner;
        protected IEnumerator<ScanCharacter> input;
        protected ScanPosition position = ScanPosition.Start;
        protected bool done;

        protected Token? current;
        public Token Current => current ?? throw new InvalidOperationException();
        object IEnumerator.Current => current ?? throw new InvalidOperationException();

        public ScannerEnumerator(Scanner scanner)
        {
            this.scanner = scanner;

            input = scanner.Source.GetEnumerator();
            done = false;
        }

        public void Dispose() { GC.SuppressFinalize(this); }
        public void Reset() { input = scanner.Source.GetEnumerator(); done = false; }
        public bool MoveNext() => (current = NextToken()) != null;

        protected virtual Token? NextToken()
        {
            if (done) return null;
            if (!input.MoveNext()) { done = true; return Token.EndOfFile(position); }

            throw NotYetImplemented();
        }
    }
}
