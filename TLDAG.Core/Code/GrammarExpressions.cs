#if DEBUG


namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        public static readonly Rex.INode GrammarRexRoot = Rex.Empty();
        public static RexData GrammarRexData => RexCompiler.Compile(GrammarRexRoot);
    }
}

#endif