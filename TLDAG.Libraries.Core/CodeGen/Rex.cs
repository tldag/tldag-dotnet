using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TLDAG.Libraries.Core.Collections;
using TLDAG.Libraries.Core.Resources;

namespace TLDAG.Libraries.Core.CodeGen
{
    public abstract class RexNode
    {
        public int Id { get; internal set; } = -1;

        private bool? nullable = null;
        public bool Nullable { get => nullable ??= GetNullable(); }

        private IntSet? firstpos = null;
        public IntSet Firstpos { get => firstpos ??= GetFirstpos(); }

        private IntSet? lastpos = null;
        public IntSet Lastpos { get => lastpos ??= GetLastpos(); }

        internal abstract void CollectLeafs(Dictionary<int, RexNode.LeafNode> leafs);
        internal abstract void CollectNodes(List<RexNode> nodes);
        internal abstract void CollectSymbols(SortedSet<char> symbols);

        internal abstract int SetIds(int nextId);
        internal abstract void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs);
        internal abstract void SetSymbolClasses(Alphabet alphabet);

        protected abstract bool GetNullable();
        protected abstract IntSet GetFirstpos();
        protected abstract IntSet GetLastpos();

        public abstract class LeafNode : RexNode
        {
            public IntSet Followpos { get; internal set; } = new();

            internal override int SetIds(int nextId)
            {
                Id = nextId;

                return nextId + 1;
            }

            protected override IntSet GetFirstpos()
                => new(Id);

            protected override IntSet GetLastpos()
                => new(Id);

            internal override void CollectLeafs(Dictionary<int, LeafNode> leafs)
            { leafs[Id] = this; }

            internal override void CollectNodes(List<RexNode> nodes)
            { nodes.Add(this); }

            internal override void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs) { }
        }

        public class Accept : LeafNode
        {
            public string Name { get; }

            internal Accept(string name)
            {
                Name = name;
            }

            protected override bool GetNullable()
                => false;

            internal override void CollectSymbols(SortedSet<char> symbols) { }
            internal override void SetSymbolClasses(Alphabet alphabet) { }
        }

        public class Empty : LeafNode
        {
            internal Empty() { }

            protected override bool GetNullable()
                => true;

            internal override void CollectSymbols(SortedSet<char> symbols) { }
            internal override void SetSymbolClasses(Alphabet alphabet) { }
        }

        public class Symbol : LeafNode
        {
            public char Char { get; }
            public int Class { get; private set; } = 0;

            internal Symbol(char c)
            {
                Char = c;
            }

            protected override bool GetNullable()
                => false;

            internal override void CollectSymbols(SortedSet<char> symbols)
            { symbols.Add(Char); }

            internal override void SetSymbolClasses(Alphabet alphabet)
            { Class = alphabet[Char]; }
        }

        public abstract class BinaryNode : RexNode
        {
            public RexNode Left { get; }
            public RexNode Right { get; }

            protected BinaryNode(RexNode left, RexNode right)
            {
                Left = left;
                Right = right;
            }

            internal override void CollectLeafs(Dictionary<int, LeafNode> leafs)
            {
                Left.CollectLeafs(leafs);
                Right.CollectLeafs(leafs);
            }

            internal override void CollectNodes(List<RexNode> nodes)
            {
                Left.CollectNodes(nodes);
                Right.CollectNodes(nodes);

                nodes.Add(this);
            }

            internal override void CollectSymbols(SortedSet<char> symbols)
            {
                Left.CollectSymbols(symbols);
                Right.CollectSymbols(symbols);
            }

            internal override int SetIds(int nextId)
            {
                nextId = Left.SetIds(nextId);
                nextId = Right.SetIds(nextId);

                return nextId;
            }

            internal override void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs)
            {
                Left.SetFollowpos(leafs);
                Right.SetFollowpos(leafs);
            }

            internal override void SetSymbolClasses(Alphabet alphabet)
            {
                Left.SetSymbolClasses(alphabet);
                Right.SetSymbolClasses(alphabet);
            }
        }

        public class Choose : BinaryNode
        {
            internal Choose(RexNode left, RexNode right)
                : base(left, right) { }

            protected override bool GetNullable()
                => Left.Nullable || Right.Nullable;

            protected override IntSet GetFirstpos()
                => Left.Firstpos + Right.Firstpos;

            protected override IntSet GetLastpos()
                => Left.Lastpos + Right.Lastpos;
        }

        public class Concat : BinaryNode
        {
            internal Concat(RexNode left, RexNode right)
                : base(left, right) { }

            protected override bool GetNullable()
                => Left.Nullable && Right.Nullable;

            protected override IntSet GetFirstpos()
                => Left.Nullable ? (Left.Firstpos + Right.Firstpos) : Left.Firstpos;

            protected override IntSet GetLastpos()
                => Right.Nullable ? (Left.Lastpos + Right.Lastpos) : Right.Lastpos;

            internal override void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs)
            {
                base.SetFollowpos(leafs);

                foreach (int id in Left.Lastpos)
                {
                    leafs[id].Followpos += Right.Firstpos;
                }
            }
        }

        public class Kleene : RexNode
        {
            public RexNode Child { get; }

            internal Kleene(RexNode child)
            {
                Child = child;
            }

            internal override int SetIds(int nextId)
                => Child.SetIds(nextId);

            internal override void CollectLeafs(Dictionary<int, LeafNode> leafs)
            { Child.CollectLeafs(leafs); }

            internal override void CollectNodes(List<RexNode> nodes)
            { Child.CollectNodes(nodes); nodes.Add(this); }

            internal override void CollectSymbols(SortedSet<char> symbols)
            { Child.CollectSymbols(symbols); }

            internal override void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs)
            {
                Child.SetFollowpos(leafs);

                foreach (int id in Lastpos)
                {
                    leafs[id].Followpos += Firstpos;
                }
            }

            internal override void SetSymbolClasses(Alphabet alphabet)
            { Child.SetSymbolClasses(alphabet); }

            protected override bool GetNullable()
                => true;

            protected override IntSet GetFirstpos()
                => Child.Firstpos;

            protected override IntSet GetLastpos()
                => Child.Lastpos;
        }
    }

    public class RexTree
    {
        public string Name { get; }
        public RexNode Root { get; }

        private Dictionary<int, RexNode.LeafNode>? leafs = null;
        public IReadOnlyDictionary<int, RexNode.LeafNode> Leafs { get => leafs ??= GetLeafs(); }

        private List<RexNode>? nodes = null;
        public IReadOnlyList<RexNode> Nodes { get => nodes ??= GetNodes(); }

        internal RexTree(string name, RexNode root)
        {
            Name = name;
            Root = root;
        }

        protected RexTree(RexNode root) : this("", root) { }

        internal int SetIds(int nextId)
            => Root.SetIds(nextId);

        internal void SetFollowpos()
        { Root.SetFollowpos(Leafs); }

        internal void CollectSymbols(SortedSet<char> symbols)
        { Root.CollectSymbols(symbols); }

        private Dictionary<int, RexNode.LeafNode> GetLeafs()
        {
            Dictionary<int, RexNode.LeafNode> leafs = new();

            Root.CollectLeafs(leafs);

            return leafs;
        }

        private List<RexNode> GetNodes()
        {
            List<RexNode> nodes = new();

            Root.CollectNodes(nodes);

            return nodes;
        }
    }

    public class RexTreeBuilder
    {
        public static readonly Regex NameRex = new("^[A-Z][_A-Z0-9]*$");

        private readonly Stack<RexNode> stack = new();

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

            RexNode left = stack.Pop();
            RexNode right = new RexNode.Accept(Name);
            RexNode root = new RexNode.Concat(left, right);

            return new(Name, root);
        }

        public RexTreeBuilder AddEmpty()
        {
            stack.Push(new RexNode.Empty());

            return this;
        }

        public RexTreeBuilder AddSymbol(char value)
        {
            stack.Push(new RexNode.Symbol(value));

            return this;
        }

        public RexTreeBuilder AddChoose()
        {
            if (stack.Count < 2)
                throw new CompilerException(CGR.SC101_RexTreeNotEnoughEntries, 2);

            RexNode right = stack.Pop();
            RexNode left = stack.Pop();

            stack.Push(new RexNode.Choose(left, right));

            return this;
        }

        public RexTreeBuilder AddConcat()
        {
            if (stack.Count < 2)
                throw new CompilerException(CGR.SC101_RexTreeNotEnoughEntries, 2);

            RexNode right = stack.Pop();
            RexNode left = stack.Pop();

            stack.Push(new RexNode.Concat(left, right));

            return this;
        }

        public RexTreeBuilder AddKleene()
        {
            if (stack.Count < 1)
                throw new CompilerException(CGR.SC101_RexTreeNotEnoughEntries, 1);

            RexNode child = stack.Pop();

            stack.Push(new RexNode.Kleene(child));

            return this;
        }
    }

    public class RexForest : RexTree
    {
        private Alphabet? alphabet = null;
        public Alphabet Alphabet { get => alphabet ??= new(GetSymbols()); }

        private Dictionary<int, string>? accepts = null;
        public IReadOnlyDictionary<int, string> Accepts { get => accepts ??= GetAccepts(); }

        internal RexForest(RexNode root) : base(root)
        {
            Root.SetSymbolClasses(Alphabet);
            Root.SetIds(1);
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
                .Where(leaf => leaf is RexNode.Accept)
                .Cast<RexNode.Accept>()
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

            RexNode[] roots = trees.Select(tree => tree.Root).ToArray();
            RexNode root = roots[0];

            for (int i = 1, n = roots.Length; i < n; ++i)
            {
                RexNode left = root;
                RexNode right = roots[i];

                root = new RexNode.Choose(left, right);
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

    public class RexCompiler
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

        private Accepting? accepting = null;
        public Accepting Accepting { get => accepting ?? throw new InvalidOperationException(); }

        private RexCompiler(RexForest forest)
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
            CreateAccepting();

            return new(Alphabet, Transitions, Accepting);
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
            TransitionsBuilder builder = new(Alphabet.Count);

            for (int i = 0; i < count; ++i)
            {
                builder.Add(ordered[i].Transitions);
            }

            transitions = builder.Build();
        }

        public static ScannerData Compile(RexForest forest)
        {
            RexCompiler compiler = new(forest);

            return compiler.Compile();
        }
    }
}
