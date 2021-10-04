using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            public Accept(string name)
            {
                Name = name;
            }

            public override Node Clone()
            {
                throw new NotSupportedException();
            }
        }

        public class Empty : Leaf
        {
            public override Node Clone()
                => new Empty();
        }

        public class Symbol : Leaf
        {
            public readonly char Value;
            public int Class = 0;

            public Symbol(char value)
            {
                Value = value;
            }

            public override Node Clone()
                => new Symbol(Value);
        }

        public abstract class Binary : Node
        {
            public readonly Node Left;
            public readonly Node Right;

            public Binary(Node left, Node right)
            {
                Left = left;
                Right = right;
            }

            public override V Visit<V>(V visitor)
            {
                Left.Visit(visitor);
                Right.Visit(visitor);

                return base.Visit(visitor);
            }
        }

        public class Choose : Binary
        {
            public Choose(Node left, Node right) : base(left, right) { }

            public override Node Clone()
                => new Choose(Left, Right);
        }

        public class Concat : Binary
        {
            public Concat(Node left, Node right) : base(left, right) { }

            public override Node Clone()
                => new Concat(Left, Right);
        }
    }
}
