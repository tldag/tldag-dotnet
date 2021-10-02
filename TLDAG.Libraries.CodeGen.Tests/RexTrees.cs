using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.CodeGen.Tests
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
