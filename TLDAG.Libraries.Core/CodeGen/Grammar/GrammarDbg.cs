using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if DEBUG

namespace TLDAG.Libraries.Core.CodeGen.Grammar
{
    public static partial class Gramm
    {
        public partial class Compiler
        {
            public Compiler(Parser parser)
            {

            }
        }
    }

    public partial class GrammarCompiler
    {
        public GrammarCompiler(Gramm.Compiler compiler)
        {
            this.compiler = compiler;
        }
    }
}

#endif