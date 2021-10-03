using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Libraries.Core.CodeGen;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.CodeGen
{
    public class ScannerCompiler
    {
        public class State
        {
            public readonly int Id;
            public readonly IntSet Positions;
            public readonly int[] Transitions;
            public readonly string? Accept;
            public string RequiredAccept => Accept ?? throw new InvalidOperationException();

            public State(int id, IntSet positions, int transitionCount, string? accept)
            {
                Id = id;
                Positions = positions;
                Transitions = new int[transitionCount];
                Accept = accept;
            }

            public State(int transitionCount) : this(0, IntSet.Empty, transitionCount, "") { }
        }

        private readonly Alphabet Alphabet;
        private readonly int transitionCount;
        private readonly RexNode root;
        private readonly IReadOnlyDictionary<int, RexNode.LeafNode> leafs;
        private readonly IReadOnlyDictionary<int, string> accepts;

        private readonly Dictionary<IntSet, State> states = new();
        private readonly Queue<State> unmarked = new();

        private Transitions? transitions = null;
        public Transitions Transitions { get => transitions ?? throw new InvalidOperationException(); }

        private AcceptingStates? acceptingStates = null;
        public AcceptingStates AcceptingStates { get => acceptingStates ?? throw new InvalidOperationException(); }

        private ScannerCompiler(RexForest forest)
        {
            Alphabet = forest.Alphabet;
            transitionCount = Alphabet.Count;
            root = forest.Root;
            leafs = forest.Leafs;
            accepts = forest.Accepts;
        }

        public ScannerData Compile()
        {
            CreateStates();
            CreateTransitions();
            CreateAcceptingStates();

            return new(Alphabet, Transitions, AcceptingStates);
        }

        private void CreateStates()
        {
            AddState(IntSet.Empty, false); // error state
            AddState(root.Firstpos); // start state

            while (unmarked.Count > 0)
            {
                ProcessState(unmarked.Dequeue());
            }
        }

        private void ProcessState(State state)
        {
            foreach (int symbol in Alphabet)
            {
                ProcessStateSymbol(state, symbol);
            }
        }

        private void ProcessStateSymbol(State state, int symbol)
        {
            if (symbol == 0) return;

            IntSet NewPositions = IntSet.Empty;

            foreach (int position in state.Positions)
            {
                if (leafs[position] is RexNode.Symbol symbolNode)
                {
                    if (symbolNode.Class == symbol)
                    {
                        NewPositions += symbolNode.Followpos;
                    }
                }
            }

            state.Transitions[symbol] = AddState(NewPositions).Id;
        }

        private State AddState(IntSet positions, bool asUnmarked = true)
        {
            if (states.ContainsKey(positions))
            {
                return states[positions];
            }

            string? accept = GetAccept(positions);
            State state = new(states.Count, positions, transitionCount, accept);

            states[positions] = state;

            if (asUnmarked)
            {
                unmarked.Enqueue(state);
            }
            
            return state;
        }

        private string? GetAccept(IntSet positions)
        {
            if (positions.Count == 0) return "";

            foreach (int position in positions)
            {
                if (accepts.ContainsKey(position))
                {
                    return accepts[position];
                }
            }

            return null;
        }

        private void CreateAcceptingStates()
        {
            acceptingStates = new(states.Values
                .Where(state => state.Accept != null)
                .ToDictionary(state => state.Id, state => state.RequiredAccept));
        }

        private void CreateTransitions()
        {
            State[] ordered = states.Values.OrderBy(state => state.Id).ToArray();
            int count = ordered.Length;
            TransitionsBuilder builder = new(Alphabet.Count);

            for (int i = 0; i < count; ++i)
            {
                builder.Add(ordered[i].Transitions);
            }

            transitions = builder.Build();
        }

        public static ScannerData Compile(RexForest forest)
        {
            ScannerCompiler compiler = new(forest);

            return compiler.Compile();
        }
    }
}
