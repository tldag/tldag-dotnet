using TLDAG.Libraries.Core.CodeGen;

namespace TLDAG.Libraries.Core.Tests.CodeGen
{
    public static class RexTrees
    {
        public static RexTree Figure_3_41()
        {
            return RexTreeBuilder.Create("FIGURE_3_41")
                .AddSymbol('a').AddSymbol('b').AddChoose()
                .AddKleene()
                .AddSymbol('a').AddConcat()
                .AddSymbol('b').AddConcat()
                .AddSymbol('b').AddConcat()
                .Build();
        }
    }
}
