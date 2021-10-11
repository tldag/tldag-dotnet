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
            private readonly int[][] transitions;

            internal Transitions(int width, int[][] transitions) { this.width = width; this.transitions = transitions; }
        }

        internal class Accepts
        {
            private readonly IntMap<string> map;

            public StringSet Values => new(map.Values);

            public Accepts(IntMap<string> map) { this.map = map; }

            public string? this[int state] { get => map[state]; }
        }

        internal class Data : Code.Rex.IData
        {
            public readonly Accepts Accepts;
            public readonly int StartState;

            public StringSet Names => Accepts.Values;

            public Data(Internal.Rex.Accepts accepts, int startState)
            {
                Accepts = accepts;
                StartState = startState;
            }

            protected Data(Data rex)
            {
                Accepts = rex.Accepts;
                StartState = rex.StartState;
            }

            public void Save(Stream stream) => throw NotYetImplemented();
            public byte[] Save(bool compress) => throw NotYetImplemented();
        }
    }
}