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

            public State(int id, IntSet positions, int transitionCount)
            {
                Id = id;
                Positions = positions;
                Transitions = new int[transitionCount];
            }

            public State(int transitionCount) : this(0, new(), transitionCount) { }
        }

        private readonly Alphabet Alphabet;
        private readonly int transitionCount;
        private readonly RexNode root;
        private readonly IReadOnlyDictionary<int, RexNode.LeafNode> leafs;

        private readonly Dictionary<IntSet, State> states = new();
        private readonly Queue<State> unmarked = new();

        private Transitions? transitions = null;
        public Transitions Transitions { get => transitions ?? throw new InvalidOperationException(); }

        private ScannerCompiler(RexForest forest)
        {
            Alphabet = forest.Alphabet;
            transitionCount = Alphabet.Count;
            root = forest.Root;
            leafs = forest.Leafs;
        }

        public ScannerData Compile()
        {
            CreateStates();
            CreateTransitions();

            return new(Alphabet, Transitions);
        }

        private void CreateStates()
        {
            AddState(new(), false); // error state
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

            IntSet NewPositions = new();

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

            State state = new(states.Count, positions, transitionCount);

            states[positions] = state;

            if (asUnmarked)
            {
                unmarked.Enqueue(state);
            }
            
            return state;
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

            this.transitions = builder.Build();
        }

        public static ScannerData Compile(RexForest forest)
        {
            ScannerCompiler compiler = new(forest);

            return compiler.Compile();
        }
    }
}
