using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.CodeGen.Scanner
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

        internal virtual int SetIds(int nextId)
            => nextId;

        internal abstract void CollectLeafs(Dictionary<int, RexNode.LeafNode> leafs);
        internal abstract void CollectNodes(List<RexNode> nodes);
        internal abstract void CollectSymbols(SortedSet<char> symbols);

        internal abstract void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs);

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
            internal Accept () { }

            protected override bool GetNullable()
                => false;

            internal override void CollectSymbols(SortedSet<char> symbols) { }
        }

        public class Empty : LeafNode
        {
            internal Empty() { }

            protected override bool GetNullable()
                => true;

            internal override void CollectSymbols(SortedSet<char> symbols) { }
        }

        public class Symbol : LeafNode
        {
            public char Value { get; }

            internal Symbol(char value)
            {
                Value = value;
            }

            protected override bool GetNullable()
                => false;

            internal override void CollectSymbols(SortedSet<char> symbols)
                { symbols.Add(Value); }
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

            internal override int SetIds(int nextId)
            {
                nextId = Left.SetIds(nextId);
                nextId = Right.SetIds(nextId);

                return nextId;
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

            internal override void SetFollowpos(IReadOnlyDictionary<int, LeafNode> leafs)
            {
                Left.SetFollowpos(leafs);
                Right.SetFollowpos(leafs);
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
            {
                Child.CollectLeafs(leafs);
            }

            internal override void CollectNodes(List<RexNode> nodes)
            {
                Child.CollectNodes(nodes);

                nodes.Add(this);
            }

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
        public RexNode.Concat Root { get; }

        private Dictionary<int, RexNode.LeafNode>? leafs = null;
        public IReadOnlyDictionary<int, RexNode.LeafNode> Leafs { get => leafs ??= GetLeafs(); }

        private List<RexNode>? nodes = null;
        public IReadOnlyList<RexNode> Nodes { get => nodes ??= GetNodes(); }

        internal RexTree(string name, RexNode.Concat root)
        {
            Name = name;
            Root = root;
        }

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
                throw new InvalidOperationException();

            RexNode left = stack.Pop();
            RexNode right = new RexNode.Accept();
            RexNode.Concat root = new(left, right);

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
                throw new InvalidOperationException();

            RexNode right = stack.Pop();
            RexNode left = stack.Pop();

            stack.Push(new RexNode.Choose(left, right));

            return this;
        }

        public RexTreeBuilder AddConcat()
        {
            if (stack.Count < 2)
                throw new InvalidOperationException();

            RexNode right = stack.Pop();
            RexNode left = stack.Pop();

            stack.Push(new RexNode.Concat(left, right));

            return this;
        }

        public RexTreeBuilder AddKleene()
        {
            if (stack.Count < 1)
                throw new InvalidOperationException();

            RexNode child = stack.Pop();

            stack.Push(new RexNode.Kleene(child));

            return this;
        }
    }

    public class RexForest : IEnumerable<RexTree>
    {
        private readonly List<RexTree> trees;

        private SortedSet<char>? symbols = null;
        public SortedSet<char> Symbols { get => new(symbols ??= GetSymbols()); }

        internal RexForest(List<RexTree> trees)
        {
            this.trees = trees;
        }

        public IEnumerator<RexTree> GetEnumerator()
            => GetTreeEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetTreeEnumerator();

        private IEnumerator<RexTree> GetTreeEnumerator()
            => trees.GetEnumerator();

        private SortedSet<char> GetSymbols()
        {
            SortedSet<char> symbols = new();

            foreach (RexTree tree in trees)
            {
                tree.CollectSymbols(symbols);
            }

            return symbols;
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
            int nextId = 1;

            foreach (RexTree tree in trees)
            {
                nextId = tree.SetIds(nextId);
                tree.SetFollowpos();
            }

            return new(trees);
        }

        public RexForestBuilder AddTree(RexTree tree)
        {
            if (names.Contains(tree.Name))
                throw new InvalidOperationException();

            trees.Add(tree);
            names.Add(tree.Name);

            return this;
        }
    }
}