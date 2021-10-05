#if DEBUG

using static TLDAG.Libraries.Core.CodeGen.Parse;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        public partial class Compiler
        {
            public Compiler(Production root)
            {

            }
        }
    }

    public partial class GrammarCompiler
    {
        public GrammarCompiler(Grammar.Compiler compiler)
        {
            this.compiler = compiler;
        }
    }
}

#endif