using System;
using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Code.Constants;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
    public partial class Rex
    {
        public interface INode { }

        public static INode Empty() => new EmptyNode();
    }

    public class RexBuilder
    {
        private Rex.Builder builder = new();

        public RexBuilder Accept(string name) { builder.Accept(name); return this; }
        public RexBuilder Empty() { builder.Empty(); return this; }
        public RexBuilder Symbol(char value) { builder.Symbol(value); return this; }
        public RexBuilder Range(char start, char end) { builder.Range(start, end); return this; }
        public RexBuilder Not(IEnumerable<char> values) { builder.Not(values); return this; }
        public RexBuilder Choose() { builder.Choose(); return this; }
        public RexBuilder Concat() { builder.Concat(); return this; }
        public RexBuilder Kleene() { builder.Kleene(); return this; }

        public Rex.INode Build() { return builder.Build(); }

        public RexBuilder A(string name) => Accept(name);
        public RexBuilder E() => Empty();
        public RexBuilder S(char value) => Symbol(value);
        public RexBuilder R(char start, char end) => Range(start, end);
        public RexBuilder N(IEnumerable<char> values) => Not(values);
        public RexBuilder CH() => Choose();
        public RexBuilder CN() => Concat();
        public RexBuilder K() => Kleene();
    }

    public partial class RexTransitions
    {
        private readonly int width;
        private readonly int[][] transitions;

        internal RexTransitions(int width, int[][] transitions) { this.width = width; this.transitions = transitions; }
    }

    public partial class RexAccepts
    {
        private readonly IntMap<string> map;

        public StringSet Values => new(map.Values);

        public RexAccepts(IntMap<string> map) { this.map = map; }

        public string? this[int state] { get => map[state]; }
    }

    public partial class RexData
    {
        public readonly RexAccepts Accepts;
        public readonly int StartState;

        public StringSet Names => Accepts.Values;

        public RexData(RexAccepts accepts, int startState)
        {
            Accepts = accepts;
            StartState = startState;
        }

        protected RexData(RexData rex)
        {
            Accepts = rex.Accepts;
            StartState = rex.StartState;
        }
    }
}
