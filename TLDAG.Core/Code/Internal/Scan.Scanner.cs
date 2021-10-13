using System;
using System.Collections;
using System.Collections.Generic;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Code.Rex;
using static TLDAG.Core.Code.Scan;
using static TLDAG.Core.Code.Internal.Rex;
using System.Text;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Scan
    {
        internal class Scanner : IEnumerable<Token>
        {
            private Data data;
            private IEnumerable<Character> source;

            public Scanner(IData data, IEnumerable<Character> source) { this.data = GetData(data); this.source = source; }
            public Scanner(IData data, string source) : this(data, Lines.Create(source)) { }

            public IEnumerator<Token> GetEnumerator() => new Enumerator(data, source);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(data, source);

            private static Data GetData(IData data) => throw NotYetImplemented();
        }

        internal class Enumerator : IEnumerator<Token>
        {
            private readonly Alphabet alphabet;
            private readonly Transitions transitions;
            private readonly Accepts accepts;

            private readonly IEnumerable<Character> characters;
            private IEnumerator<Character> input;
            private Position position = Position.Start;
            private bool done;

            private Token? current;
            public Token Current => current ?? throw new InvalidOperationException();
            object IEnumerator.Current => current ?? throw new InvalidOperationException();

            public Enumerator(Data data, IEnumerable<Character> characters)
            {
                alphabet = data.Alphabet;
                transitions = data.Transitions;
                accepts = data.Accepts;

                this.characters = characters;

                input = characters.GetEnumerator();
                done = false;
            }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { input = characters.GetEnumerator(); done = false; }
            public bool MoveNext() => (current = NextToken()) != null;

            private Token? NextToken()
            {
                if (done) return null;
                if (!input.MoveNext()) { done = true; return Token.EndOfFile(position); }

                uint state = 1;
                Character current = input.Current;
                StringBuilder value = new();

                position = current.Position;

                while (true)
                {
                    char c = current.Value;
                    uint cc = alphabet[c];

                    value.Append(c);
                    state = transitions[state, cc];

                    if (state == 0)
                        throw NotYetImplemented();

                    string? accept = accepts[state];

                    if (accept is not null)
                        return new(position, accept, value.ToString());

                    current = input.Current;
                }
            }
        }
    }
}