using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Algorithms;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Code
{
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

    internal class RexGetAlphabet : Rex.IVisitor
    {
        private readonly List<char> symbols = new();

        public void Visit(Rex.Node node)
        {
            if (node is Rex.SymbolNode symbolNode) symbols.Add(symbolNode.Value);
            else if (node is Rex.NotNode notNode) throw NotYetImplemented();
        }

        public static Alphabet GetAlphabet(Rex.Node root)
        {
            RexGetAlphabet visitor = new();

            root.VisitDepthFirst(visitor);

            return new(visitor.symbols);
        }
    }

    internal class RexExpandTree : Rex.IVisitor
    {
        public void Visit(Rex.Node node)
        {
            if (node is Rex.BinaryNode binaryNode) ExpandBinary(binaryNode);
            else if (node is Rex.KleeneNode kleeneNode) ExpandKleene(kleeneNode);
        }

        private static void ExpandBinary(Rex.BinaryNode binaryNode)
        { binaryNode.Left = InternalExpand(binaryNode.Left); binaryNode.Right = InternalExpand(binaryNode.Right); }

        private static void ExpandKleene(Rex.KleeneNode kleeneNode)
        { kleeneNode.Child = InternalExpand(kleeneNode.Child); }

        private static Rex.Node InternalExpand(Rex.Node node)
        {
            if (node is Rex.NotNode notNode) return ExpandNotNode(notNode);

            return node;
        }

        private static Rex.Node ExpandNotNode(Rex.NotNode notNode)
        {
            throw NotYetImplemented();
        }

        public static Rex.Node Expand(Rex.Node root, Alphabet alphabet)
        {
            RexExpandTree visitor = new();

            root.VisitPreOrder(visitor);

            return InternalExpand(root);
        }
    }

    internal class RexInitTree : Rex.IVisitor
    {
        private int nextId = 1;

        public void Visit(Rex.Node node)
        {
            if (node is Rex.LeafNode leafNode) InitLeaf(leafNode);
            if (node is Rex.AcceptNode acceptNode) InitAccept(acceptNode);
            if (node is Rex.EmptyNode emptyNode) InitEmpty(emptyNode);
            if (node is Rex.SymbolNode symbolNode) InitSymbol(symbolNode);
            if (node is Rex.ChooseNode chooseNode) InitChoose(chooseNode);
            if (node is Rex.ConcatNode concatNode) InitConcat(concatNode);
            if (node is Rex.KleeneNode kleeneNode) InitKleene(kleeneNode);
            if (node is Rex.NotNode) throw new NotSupportedException();
        }

        private void InitLeaf(Rex.LeafNode leafNode) { leafNode.Id = nextId++; }

        private void InitAccept(Rex.AcceptNode acceptNode)
        {
            throw NotYetImplemented();
        }

        private void InitEmpty(Rex.EmptyNode emptyNode) { emptyNode.Nullable = true; }

        private void InitSymbol(Rex.SymbolNode symbolNode)
        {
            throw NotYetImplemented();
        }

        private void InitChoose(Rex.ChooseNode chooseNode)
        {
            throw NotYetImplemented();
        }

        private void InitConcat(Rex.ConcatNode concatNode)
        {
            throw NotYetImplemented();
        }

        private void InitKleene(Rex.KleeneNode kleeneNode)
        {
            throw NotYetImplemented();
        }

        public static void Init(Rex.Node root)
        {
            RexInitTree init = new();

            root.VisitDepthFirst(init);
        }
    }

    public class RexState
    {
        public readonly int Id;
        public readonly string? Accept;
        private readonly int[] transitions;

        public int[] Transitions => Arrays.Copy(transitions);

        public RexState(int id, string? accept, int width)
        {
            Id = id;
            Accept = accept;
            transitions = new int[width];
        }
    }

    public class RexCompiler
    {
        private readonly Alphabet alphabet;
        private readonly int width;

        private readonly Rex.Node root;

        private readonly SmartMap<IntSet, RexState> states = new();
        private readonly Queue<RexState> unmarked = new();

        public RexCompiler(Rex.INode root)
        {
            this.root = root as Rex.Node ?? throw new NotSupportedException();

            alphabet = RexGetAlphabet.GetAlphabet(this.root);
            width = alphabet.Count;

            this.root = RexExpandTree.Expand(this.root, alphabet);

            RexInitTree.Init(this.root);
        }

        public RexData Compile()
        {
            CreateStates();

            RexTransitions transitions = CreateTransitions();
            RexAccepts accepts = CreateAccepts();

            return new(accepts, 1);
        }

        private void CreateStates()
        {
            AddState(IntSet.Empty, false);
            AddState(root.Firstpos);

            while (unmarked.Count > 0) ProcessState(unmarked.Dequeue());
        }

        private void ProcessState(RexState state)
        {
            throw NotYetImplemented();
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
                throw NotYetImplemented();
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

        public static RexCompiler Create(Rex.INode root) => new(root);
        public static RexData Compile(Rex.INode root) => Create(root).Compile();
    }
}
