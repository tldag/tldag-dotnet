using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Collections;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Rex
    {
        public abstract class Node
        {
            public interface IVisitor
            {
                public void Visit(Node node);
            }

            public bool Nullable = false;
            public IntSetOld Firstpos = IntSetOld.Empty;
            public IntSetOld Lastpos = IntSetOld.Empty;

            public virtual V Visit<V>(V visitor) where V : IVisitor
                { visitor.Visit(this); return visitor; }

            public abstract Node Clone();
        }

        public abstract class Leaf : Node
        {
            public int Id = 0;
            public IntSetOld Follopos = IntSetOld.Empty;
        }

        public class Accept : Leaf
        {
            public readonly string Name;

            public Accept(string name) { Name = name; }

            public override Node Clone() { throw new NotSupportedException(); }
        }

        public class Empty : Leaf
        {
            public override Node Clone() => new Empty();
        }

        public class Symbol : Leaf
        {
            public readonly char Value;

            public Symbol(char value) { Value = value; }

            public override Node Clone() => new Symbol(Value);
        }

        public abstract class Binary : Node
        {
            public readonly Node Left;
            public readonly Node Right;

            public Binary(Node left, Node right) { Left = left; Right = right; }

            public override V Visit<V>(V visitor)
                { Left.Visit(visitor); Right.Visit(visitor); return base.Visit(visitor); }
        }

        public class Choose : Binary
        {
            public Choose(Node left, Node right) : base(left, right) { }

            public override Node Clone() => new Choose(Left.Clone(), Right.Clone());
        }

        public class Concat : Binary
        {
            public Concat(Node left, Node right) : base(left, right) { }

            public override Node Clone() => new Concat(Left.Clone(), Right.Clone());
        }

        public class Kleene : Node
        {
            public readonly Node Child;

            public Kleene(Node child) { Child = child; }

            public override Node Clone() => new Kleene(Child.Clone());
        }

        public class Builder
        {
            private readonly Stack<Node> stack = new();
            private readonly HashSet<string> names = new(Code.ReservedTokenNames, StringComparer.Ordinal);

            public Builder Accept(string name)
            {
                if (names.Contains(name)) throw new ArgumentException("Duplicate name");
                if (!Code.TokenNameRegex.IsMatch(name)) throw new ArgumentException("Illegal name");

                names.Add(name); stack.Push(new Accept(name)); return this;
            }

            public Builder Empty() { stack.Push(new Empty()); return this; }
            public Builder Symbol(char value) { stack.Push(new Symbol(value)); return this; }

            public Builder Choose()
            { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new Choose(left, right)); return this; }

            public Builder Concat()
            { Node right = stack.Pop(); Node left = stack.Pop(); stack.Push(new Concat(left, right)); return this; }

            public Builder Kleene() { stack.Push(new Kleene(stack.Pop())); return this; }

            public Node Build()
            {
                if (stack.Count != 1) throw new InvalidOperationException();

                return stack.Pop();
            }
        }
    }
}
