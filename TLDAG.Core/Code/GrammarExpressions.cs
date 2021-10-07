#if DEBUG


namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        public static readonly RexNode GrammarRexRoot = new RexEmptyNode();
        public static RexData GrammarRexData => RexCompiler.Compile(GrammarRexRoot);
    }
}

#endif