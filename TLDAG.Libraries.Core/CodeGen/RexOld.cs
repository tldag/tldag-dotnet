using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TLDAG.Libraries.Core.Collections;
using TLDAG.Libraries.Core.Resources;

namespace TLDAG.Libraries.Core.CodeGen
{
    public abstract class RexNodeOld
    {
        public interface IVisitor { void Visit(RexNodeOld node); }

        public bool Nullable { get; internal set; } = false;
        public IntSetOld Firstpos { get; internal set; } = IntSetOld.Empty;
        public IntSetOld Lastpos { get; internal set; } = IntSetOld.Empty;

        public virtual V Visit<V>(V visitor) where V : IVisitor
            { visitor.Visit(this); return visitor; }

        internal abstract void CollectSymbols(SortedSet<char> symbols);

        internal abstract void SetFollowpos(IReadOnlyDictionary<int, Leaf> leafs);
        internal abstract void SetSymbolClasses(AlphabetOld alphabet);

        public abstract class Leaf : RexNodeOld
        {
            public int Id { get; internal set; } = -1;
            public IntSetOld Followpos { get; internal set; } = new();

            internal override void SetFollowpos(IReadOnlyDictionary<int, Leaf> leafs) { }
        }

        public class Accept : Leaf
        {
            public string Name { get; }

            internal Accept(string name)
            {
                Name = name;
            }

            internal override void CollectSymbols(SortedSet<char> symbols) { }
            internal override void SetSymbolClasses(AlphabetOld alphabet) { }
        }

        public class Empty : Leaf
        {
            internal Empty() { }

            internal override void CollectSymbols(SortedSet<char> symbols) { }
            internal override void SetSymbolClasses(AlphabetOld alphabet) { }
        }

        public class Symbol : Leaf
        {
            public char Char { get; }
            public int Class { get; private set; } = 0;

            internal Symbol(char c)
            {
                Char = c;
            }

            internal override void CollectSymbols(SortedSet<char> symbols)
            { symbols.Add(Char); }

            internal override void SetSymbolClasses(AlphabetOld alphabet)
            { Class = alphabet[Char]; }
        }

        public abstract class BinaryNode : RexNodeOld
        {
            public RexNodeOld Left { get; }
            public RexNodeOld Right { get; }

            protected BinaryNode(RexNodeOld left, RexNodeOld right)
            {
                Left = left;
                Right = right;
            }

            public override V Visit<V>(V visitor)
                { Left.Visit(visitor); Right.Visit(visitor); return base.Visit(visitor); }

            internal override void CollectSymbols(SortedSet<char> symbols)
            {
                Left.CollectSymbols(symbols);
                Right.CollectSymbols(symbols);
            }

            internal override void SetFollowpos(IReadOnlyDictionary<int, Leaf> leafs)
            {
                Left.SetFollowpos(leafs);
                Right.SetFollowpos(leafs);
            }

            internal override void SetSymbolClasses(AlphabetOld alphabet)
            {
                Left.SetSymbolClasses(alphabet);
                Right.SetSymbolClasses(alphabet);
            }
        }

        public class Choose : BinaryNode
        {
            internal Choose(RexNodeOld left, RexNodeOld right)
                : base(left, right) { }
        }

        public class Concat : BinaryNode
        {
            internal Concat(RexNodeOld left, RexNodeOld right)
                : base(left, right) { }

            internal override void SetFollowpos(IReadOnlyDictionary<int, Leaf> leafs)
            {
                base.SetFollowpos(leafs);

                foreach (int id in Left.Lastpos)
                {
                    leafs[id].Followpos += Right.Firstpos;
                }
            }
        }

        public class Kleene : RexNodeOld
        {
            public RexNodeOld Child { get; }

            internal Kleene(RexNodeOld child)
            {
                Child = child;
            }

            public override V Visit<V>(V visitor)
                { Child.Visit(visitor); return base.Visit(visitor); }

            internal override void CollectSymbols(SortedSet<char> symbols)
            { Child.CollectSymbols(symbols); }

            internal override void SetFollowpos(IReadOnlyDictionary<int, Leaf> leafs)
            {
                Child.SetFollowpos(leafs);

                foreach (int id in Lastpos)
                {
                    leafs[id].Followpos += Firstpos;
                }
            }

            internal override void SetSymbolClasses(AlphabetOld alphabet)
            { Child.SetSymbolClasses(alphabet); }
        }
    }

    public class RexTree
    {
        public string Name { get; }
        public RexNodeOld Root { get; }

        internal RexTree(string name, RexNodeOld root)
        {
            Name = name;
            Root = root;
        }

        protected RexTree(RexNodeOld root) : this("", root) { }

        internal void CollectSymbols(SortedSet<char> symbols)
            { Root.CollectSymbols(symbols); }
    }

    public class RexTreeBuilder
    {
        public static readonly Regex NameRex = new("^[A-Z][_A-Z0-9]*$");

        private readonly Stack<RexNodeOld> stack = new();

        public string Name { get; }

        private RexTreeBuilder(string name)
        {
            Name = name;
        }

        public static RexTreeBuilder Create(string name)
        {
            if (!NameRex.IsMatch(name))
                throw new NotSupportedException();

            return new(name);
        }

        public RexTree Build()
        {
            if (stack.Count != 1)
                throw new CompilerException(CGR.SC100_RexTreeStackNotOne);

            RexNodeOld left = stack.Pop();
            RexNodeOld right = new RexNodeOld.Accept(Name);
            RexNodeOld root = new RexNodeOld.Concat(left, right);

            return new(Name, root);
        }

        public RexTreeBuilder AddEmpty()
        {
            stack.Push(new RexNodeOld.Empty());

            return this;
        }

        public RexTreeBuilder AddSymbol(char value)
        {
            stack.Push(new RexNodeOld.Symbol(value));

            return this;
        }

        public RexTreeBuilder AddChoose()
        {
            if (stack.Count < 2)
                throw new CompilerException(CGR.SC101_RexTreeNotEnoughEntries, 2);

            RexNodeOld right = stack.Pop();
            RexNodeOld left = stack.Pop();

            stack.Push(new RexNodeOld.Choose(left, right));

            return this;
        }

        public RexTreeBuilder AddConcat()
        {
            if (stack.Count < 2)
                throw new CompilerException(CGR.SC101_RexTreeNotEnoughEntries, 2);

            RexNodeOld right = stack.Pop();
            RexNodeOld left = stack.Pop();

            stack.Push(new RexNodeOld.Concat(left, right));

            return this;
        }

        public RexTreeBuilder AddKleene()
        {
            if (stack.Count < 1)
                throw new CompilerException(CGR.SC101_RexTreeNotEnoughEntries, 1);

            RexNodeOld child = stack.Pop();

            stack.Push(new RexNodeOld.Kleene(child));

            return this;
        }
    }

    public class RexForest : RexTree
    {
        public class SetNullableVisitor : RexNodeOld.IVisitor
        {
            public void Visit(RexNodeOld node)
            {
                if (node is RexNodeOld.Empty) node.Nullable = true;
                else if (node is RexNodeOld.Choose choose) node.Nullable = choose.Left.Nullable || choose.Right.Nullable;
                else if (node is RexNodeOld.Concat concat) node.Nullable = concat.Left.Nullable && concat.Right.Nullable;
                else if (node is RexNodeOld.Kleene) node.Nullable = true;
            }
        }

        public class SetIdVisitor : RexNodeOld.IVisitor
        {
            private int nextId = 1;

            public void Visit(RexNodeOld node)
            {
                if (node is RexNodeOld.Leaf leaf)
                {
                    leaf.Id = nextId;
                    ++nextId;
                }
            }
        }

        public class SetFirstposVisitor : RexNodeOld.IVisitor
        {
            public void Visit(RexNodeOld node)
            {
                if (node is RexNodeOld.Leaf leaf)
                    node.Firstpos = new(leaf.Id);

                else if (node is RexNodeOld.Choose choose)
                    node.Firstpos = choose.Left.Firstpos + choose.Right.Firstpos;

                else if (node is RexNodeOld.Concat concat)
                    node.Firstpos = concat.Left.Nullable ? (concat.Left.Firstpos + concat.Right.Firstpos) : concat.Left.Firstpos;

                else if (node is RexNodeOld.Kleene kleene)
                    node.Firstpos = kleene.Child.Firstpos;
            }
        }

        public class SetLastposVisitor : RexNodeOld.IVisitor
        {
            public void Visit(RexNodeOld node)
            {
                if (node is RexNodeOld.Leaf leaf)
                    node.Lastpos = new(leaf.Id);

                else if (node is RexNodeOld.Choose choose)
                    node.Lastpos = choose.Left.Lastpos + choose.Right.Lastpos;

                else if (node is RexNodeOld.Concat concat)
                    node.Lastpos = concat.Right.Nullable ? (concat.Left.Lastpos + concat.Right.Lastpos) : concat.Right.Lastpos;

                else if (node is RexNodeOld.Kleene kleene)
                    node.Lastpos = kleene.Child.Lastpos;
            }
        }

        public class CollectNodesVisitor : RexNodeOld.IVisitor
        {
            private readonly List<RexNodeOld> nodes;

            public CollectNodesVisitor(List<RexNodeOld> nodes)
                { this.nodes = nodes; }

            public void Visit(RexNodeOld node)
                { nodes.Add(node); }
        }

        public class CollectLeafsVisitor : RexNodeOld.IVisitor
        {
            private readonly Dictionary<int, RexNodeOld.Leaf> leafs;

            public CollectLeafsVisitor(Dictionary<int, RexNodeOld.Leaf> leafs)
                { this.leafs = leafs; }

            public void Visit(RexNodeOld node)
                { if (node is RexNodeOld.Leaf leaf) leafs[leaf.Id] = leaf; }
        }

        private AlphabetOld? alphabet = null;
        public AlphabetOld Alphabet { get => alphabet ??= new(GetSymbols()); }

        private readonly List<RexNodeOld> nodes = new();
        public IReadOnlyList<RexNodeOld> Nodes { get => nodes; }

        private readonly Dictionary<int, RexNodeOld.Leaf> leafs = new();
        public IReadOnlyDictionary<int, RexNodeOld.Leaf> Leafs { get => leafs; }

        private Dictionary<int, string>? accepts = null;
        public IReadOnlyDictionary<int, string> Accepts { get => accepts ??= GetAccepts(); }

        internal RexForest(RexNodeOld root) : base(root)
        {
            Root.SetSymbolClasses(Alphabet);
            Root.Visit(new SetIdVisitor());
            Root.Visit(new SetNullableVisitor());
            Root.Visit(new SetFirstposVisitor());
            Root.Visit(new SetLastposVisitor());
            Root.Visit(new CollectNodesVisitor(nodes));
            Root.Visit(new CollectLeafsVisitor(leafs));
            Root.SetFollowpos(Leafs);
        }

        private SortedSet<char> GetSymbols()
        {
            SortedSet<char> symbols = new();

            Root.CollectSymbols(symbols);

            return symbols;
        }

        private Dictionary<int, string> GetAccepts()
        {
            return Leafs.Values
                .Where(leaf => leaf is RexNodeOld.Accept)
                .Cast<RexNodeOld.Accept>()
                .ToDictionary(accept => accept.Id, accept => accept.Name);
        }
    }

    public class RexForestBuilder
    {
        private readonly List<RexTree> trees = new();
        private readonly HashSet<string> names = new();

        private RexForestBuilder() { }

        public static RexForestBuilder Create()
            => new();

        public RexForest Build()
        {
            if (trees.Count < 1)
                throw new CompilerException(CGR.SC102_RexForestEmpty);

            RexNodeOld[] roots = trees.Select(tree => tree.Root).ToArray();
            RexNodeOld root = roots[0];

            for (int i = 1, n = roots.Length; i < n; ++i)
            {
                RexNodeOld left = root;
                RexNodeOld right = roots[i];

                root = new RexNodeOld.Choose(left, right);
            }

            return new(root);
        }

        public RexForestBuilder AddTree(RexTree tree)
        {
            if (names.Contains(tree.Name))
                throw new CompilerException(CGR.SC103_RexForestDuplicateName, tree.Name);

            trees.Add(tree);
            names.Add(tree.Name);

            return this;
        }
    }

    public class RexCompilerOld
    {
        public class State
        {
            public readonly int Id;
            public readonly IntSetOld Positions;
            public readonly int[] Transitions;
            public readonly string? Accept;
            public string RequiredAccept => Accept ?? throw new InvalidOperationException();

            public State(int id, IntSetOld positions, int transitionCount, string? accept)
            {
                Id = id;
                Positions = positions;
                Transitions = new int[transitionCount];
                Accept = accept;
            }

            public State(int transitionCount) : this(0, IntSetOld.Empty, transitionCount, "") { }
        }

        private readonly AlphabetOld Alphabet;
        private readonly int transitionCount;
        private readonly RexNodeOld root;
        private readonly IReadOnlyDictionary<int, RexNodeOld.Leaf> leafs;
        private readonly IReadOnlyDictionary<int, string> accepts;

        private readonly Dictionary<IntSetOld, State> states = new();
        private readonly Queue<State> unmarked = new();

        private TransitionsOld? transitions = null;
        public TransitionsOld Transitions { get => transitions ?? throw new InvalidOperationException(); }

        private AcceptingOld? accepting = null;
        public AcceptingOld Accepting { get => accepting ?? throw new InvalidOperationException(); }

        private RexCompilerOld(RexForest forest)
        {
            Alphabet = forest.Alphabet;
            transitionCount = Alphabet.Count;
            root = forest.Root;
            leafs = forest.Leafs;
            accepts = forest.Accepts;
        }

        public ScannerDataOld Compile()
        {
            CreateStates();
            CreateTransitions();
            CreateAccepting();

            return new(Alphabet, Transitions, Accepting);
        }

        private void CreateStates()
        {
            AddState(IntSetOld.Empty, false); // error state
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

            IntSetOld NewPositions = IntSetOld.Empty;

            foreach (int position in state.Positions)
            {
                if (leafs[position] is RexNodeOld.Symbol symbolNode)
                {
                    if (symbolNode.Class == symbol)
                    {
                        NewPositions += symbolNode.Followpos;
                    }
                }
            }

            state.Transitions[symbol] = AddState(NewPositions).Id;
        }

        private State AddState(IntSetOld positions, bool asUnmarked = true)
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

        private string? GetAccept(IntSetOld positions)
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

        private void CreateAccepting()
        {
            accepting = new(states.Values
                .Where(state => state.Accept != null)
                .ToDictionary(state => state.Id, state => state.RequiredAccept));
        }

        private void CreateTransitions()
        {
            State[] ordered = states.Values.OrderBy(state => state.Id).ToArray();
            int count = ordered.Length;
            TransitionsBuilderOld builder = new(Alphabet.Count);

            for (int i = 0; i < count; ++i)
            {
                builder.Add(ordered[i].Transitions);
            }

            transitions = builder.Build();
        }

        public static ScannerDataOld Compile(RexForest forest)
        {
            RexCompilerOld compiler = new(forest);

            return compiler.Compile();
        }
    }
}
