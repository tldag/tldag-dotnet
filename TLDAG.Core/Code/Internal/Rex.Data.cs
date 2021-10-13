using System.IO;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Rex
    {
        internal class Transitions
        {
            private readonly int width;
            private readonly uint[][] transitions;

            internal Transitions(int width, uint[][] transitions)
                { this.width = width; this.transitions = transitions; }

            public uint this[uint state, uint symbol] => throw NotYetImplemented();
        }

        internal class Accepts
        {
            private readonly UIntMap<string> map;

            public UIntSet Keys => new(map.Keys);
            public StringSet Values => new(map.Values);

            public Accepts(UIntMap<string> map) { this.map = new(map); }

            public string? this[uint state] { get => map[state]; }
        }

        internal class Data : Code.Rex.IData
        {
            public readonly Alphabet Alphabet;
            public readonly Transitions Transitions;
            public readonly Accepts Accepts;
            public readonly int StartState;

            public StringSet Names => Accepts.Values;

            public Data(Alphabet alphabet, Transitions transitions, Accepts accepts, int startState)
            {
                Alphabet = alphabet;
                Transitions = transitions;
                Accepts = accepts;
                StartState = startState;
            }

            protected Data(Data rex)
            {
                Alphabet = rex.Alphabet;
                Transitions = rex.Transitions;
                Accepts = rex.Accepts;
                StartState = rex.StartState;
            }

            public void Save(Stream stream) => throw NotYetImplemented();
            public byte[] Save(bool compress) => throw NotYetImplemented();
        }
    }
}