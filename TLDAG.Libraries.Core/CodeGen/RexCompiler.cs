using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Libraries.Core.Algorithms;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public partial class RexTransitions
    {
        private readonly int width;
        private readonly int[][] transitions;

        internal RexTransitions(int width, int[][] transitions) { this.width = width; this.transitions = transitions; }
    }

    public class RexTransitionsBuilder
    {
        private readonly int width;
        private readonly List<int[]> list = new();

        public RexTransitionsBuilder(int width) { this.width = width; }
        public static RexTransitionsBuilder Create(int width) => new(width);

        public RexTransitionsBuilder Add(int[] transitions)
        {
            if (transitions.Length != width) throw new ArgumentException();
            list.Add(transitions); return this;
        }

        public RexTransitions Build() => new(width, list.ToArray());
    }

    public partial class RexAccepts
    {
        private readonly IntMap<string> map;

        public RexAccepts(IntMap<string> map) { this.map = map; }

        public string? this[int state] { get => map[state]; }
    }

    public partial class RexData
    {
        public StringSet Names => throw new NotImplementedException();

        public RexData() { }
        protected RexData(RexData rex) { }
    }

    public class RexExpandTree : IRexNodeVisitor
    {
        public void Visit(RexNode node)
        {
            if (node is RexBinaryNode binaryNode) ExpandBinary(binaryNode);
            if (node is RexKleeneNode kleeneNode) ExpandKleene(kleeneNode);
        }

        private static void ExpandBinary(RexBinaryNode binaryNode)
        { binaryNode.Left = InternalExpand(binaryNode.Left); binaryNode.Right = InternalExpand(binaryNode.Right); }

        private static void ExpandKleene(RexKleeneNode kleeneNode)
        { kleeneNode.Child = InternalExpand(kleeneNode.Child); }

        private static RexNode InternalExpand(RexNode node)
        {
            // TODO: Expand RexNotNode

            return node;
        }

        public static RexNode Expand(RexNode root)
        {
            RexExpandTree visitor = new();

            root.VisitPreOrder(visitor);

            return InternalExpand(root);
        }
    }

    public class RexInitTree : IRexNodeVisitor
    {
        private int nextId = 1;

        public readonly List<char> Symbols = new();

        public void Visit(RexNode node)
        {
            if (node is RexLeafNode leafNode) InitLeaf(leafNode);
            if (node is RexAcceptNode acceptNode) InitAccept(acceptNode);
            if (node is RexEmptyNode emptyNode) InitEmpty(emptyNode);
            if (node is RexSymbolNode symbolNode) InitSymbol(symbolNode);
            if (node is RexChooseNode chooseNode) InitChoose(chooseNode);
            if (node is RexConcatNode concatNode) InitConcat(concatNode);
            if (node is RexKleeneNode kleeneNode) InitKleene(kleeneNode);
        }

        private void InitLeaf(RexLeafNode leafNode) { leafNode.Id = nextId++; }

        private void InitAccept(RexAcceptNode acceptNode)
        {
            throw new NotImplementedException();
        }

        private void InitEmpty(RexEmptyNode emptyNode) { emptyNode.Nullable = true; }

        private void InitSymbol(RexSymbolNode symbolNode)
        {
            throw new NotImplementedException();
        }

        private void InitChoose(RexChooseNode chooseNode)
        {
            throw new NotImplementedException();
        }

        private void InitConcat(RexConcatNode concatNode)
        {
            throw new NotImplementedException();
        }

        private void InitKleene(RexKleeneNode kleeneNode)
        {
            throw new NotImplementedException();
        }

        public static RexInitTree Init(RexNode root)
        {
            RexInitTree init = new();

            root.VisitDepthFirst(init);

            return init;
        }
    }

    public class RexState
    {
        public readonly int Id;
        public readonly string? Accept;
        private readonly int[] transitions;

        public int[] Transitions => ArrayUtils.Copy(transitions);

        public RexState(int id, string? accept, int width)
        {
            Id = id;
            Accept = accept;
            transitions = new int[width];
        }
    }

    public class RexCompiler
    {
        private readonly RexNode root;

        private readonly Alphabet alphabet;
        private readonly int width;

        private readonly SmartMap<IntSet, RexState> states = new();
        private readonly Queue<RexState> unmarked = new();

        public RexCompiler(RexNode root)
        {
            this.root = RexExpandTree.Expand(root);

            RexInitTree init = RexInitTree.Init(root);

            alphabet = new(init.Symbols);
            width = alphabet.Count;
        }

        public RexData Compile()
        {
            CreateStates();

            RexTransitions transitions = CreateTransitions();
            RexAccepts accepts = CreateAccepts();

            return new();
        }

        private void CreateStates()
        {
            AddState(IntSet.Empty, false);
            AddState(root.Firstpos);

            while (unmarked.Count > 0) ProcessState(unmarked.Dequeue());
        }

        private void ProcessState(RexState state)
        {
            throw new NotImplementedException();
        }

        private RexState AddState(IntSet positions, bool asUnmarked = true)
        {
            RexState? state = states[positions];

            if (state == null)
            {
                string? accept = GetAccept(positions);

                state = new(states.Count, accept, width);
                states[positions] = state;
                if (asUnmarked) unmarked.Enqueue(state);
            }

            return state;
        }

        private string? GetAccept(IntSet positions)
        {
            if (positions.Count == 0) return "";

            foreach (int position in positions)
            {
                throw new NotImplementedException();
            }

            return null;
        }

        private RexTransitions CreateTransitions()
        {
            RexState[] ordered = states.Values.OrderBy(state => state.Id).ToArray();
            int count = ordered.Length;
            RexTransitionsBuilder builder = new(width);

            for (int i = 0; i < count; ++i) builder.Add(ordered[i].Transitions);

            return builder.Build();
        }

        private RexAccepts CreateAccepts()
        {
            IntMap<string> accepts = new();

            foreach (RexState state in states.Values)
            {
                string? accept = state.Accept;

                if (accept is not null) accepts[state.Id] = accept;
            }

            return new(accepts);
        }

        public static RexCompiler Create(RexNode root) => new(root);
        public static RexData Compile(RexNode root) => Create(root).Compile();
    }
}
