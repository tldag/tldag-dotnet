﻿using System.Collections.Generic;
using TLDAG.Core.Collections;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Code.Constants;
using System;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Rex
    {
        internal abstract class Node : Code.Rex.INode
        {
            private bool? nullable = null;
            public bool Nullable => nullable ??= ComputeNullable();

            private UIntSet? firstpos = null;
            public UIntSet Firstpos => firstpos ??= ComputeFirstpos();

            private UIntSet? lastpos = null;
            public UIntSet Lastpos => lastpos ??= ComputeLastpos();

            public virtual V VisitDepthFirst<V>(V visitor) where V : Code.Rex.IVisitor => Visit(visitor);
            public virtual V VisitPreOrder<V>(V visitor) where V : Code.Rex.IVisitor => Visit(visitor);
            protected virtual V Visit<V>(V visitor) where V : Code.Rex.IVisitor { visitor.Visit(this); return visitor; }

            public abstract Node Clone();

            protected abstract bool ComputeNullable();
            protected abstract UIntSet ComputeFirstpos();
            protected abstract UIntSet ComputeLastpos();
        }

        internal abstract class LeafNode : Node
        {
            public uint Id = 0;
            public UIntSet Followpos = UIntSet.Empty;
        }

        internal class AcceptNode : LeafNode
        {
            public readonly string Name;

            public AcceptNode(string name) { Name = name; }

            public override Node Clone() { throw NotSupported(); }

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal class EmptyNode : LeafNode
        {
            public override Node Clone() => new EmptyNode();

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal class SymbolNode : LeafNode
        {
            public readonly char Value;

            public SymbolNode(char value) { Value = value; }

            public override Node Clone() => new SymbolNode(Value);

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal class NotNode : Node
        {
            public override Node Clone() => throw NotYetImplemented();

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal abstract class BinaryNode : Node
        {
            public Node Left;
            public Node Right;

            public BinaryNode(Node left, Node right) { Left = left; Right = right; }

            public override V VisitDepthFirst<V>(V visitor)
            { Left.VisitDepthFirst(visitor); Right.VisitDepthFirst(visitor); return Visit(visitor); }

            public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Left.VisitPreOrder(visitor); Right.VisitPreOrder(visitor); return visitor; }
        }

        internal class ChooseNode : BinaryNode
        {
            public ChooseNode(Node left, Node right) : base(left, right) { }

            public override Node Clone() => new ChooseNode(Left.Clone(), Right.Clone());

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal class ConcatNode : BinaryNode
        {
            public ConcatNode(Node left, Node right) : base(left, right) { }

            public override Node Clone() => new ConcatNode(Left.Clone(), Right.Clone());

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal class KleeneNode : Node
        {
            public Node Child;

            public KleeneNode(Node child) { Child = child; }

            public override V VisitDepthFirst<V>(V visitor)
            { Child.VisitDepthFirst(visitor); return Visit(visitor); }

            public override V VisitPreOrder<V>(V visitor)
            { Visit(visitor); Child.VisitPreOrder(visitor); return visitor; }

            public override Node Clone() => new KleeneNode(Child.Clone());

            protected override bool ComputeNullable() => throw NotYetImplemented();
            protected override UIntSet ComputeFirstpos() => throw NotYetImplemented();
            protected override UIntSet ComputeLastpos() => throw NotYetImplemented();
        }

        internal class Builder
        {
            private readonly Stack<Node> stack = new();
            private readonly HashSet<string> names = new(ReservedTokenNames, StringComparer.Ordinal);

            public void Accept(string name)
            {
                if (names.Contains(name)) throw new ArgumentException("Duplicate name");
                if (!TokenNameRegex.IsMatch(name)) throw new ArgumentException("Illegal name");

                names.Add(name); stack.Push(new AcceptNode(name));
            }

            public void Empty() { stack.Push(new EmptyNode()); }
            public void Symbol(char value) { stack.Push(new SymbolNode(value)); }
            public void Range(char start, char end) { throw NotYetImplemented(); }
            public void Not(IEnumerable<char> values) { throw NotYetImplemented(); }
            public void Choose() { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new ChooseNode(left, right)); }
            public void Concat() { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new ConcatNode(left, right)); }
            public void Kleene() { stack.Push(new KleeneNode(stack.Pop())); }

            public Node Build()
            {
                if (stack.Count == 0) return new EmptyNode();
                if (stack.Count > 1) throw new InvalidOperationException();

                return stack.Pop();
            }
        }
    }
}