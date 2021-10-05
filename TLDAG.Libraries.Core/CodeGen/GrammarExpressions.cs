#if DEBUG

using System;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        public static readonly RexNode GrammarRexRoot = new RexEmptyNode();
        public static RexData GrammarRexData => RexCompiler.Compile(GrammarRexRoot);
    }
}

#endif