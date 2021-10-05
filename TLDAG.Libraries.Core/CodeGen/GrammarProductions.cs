#if DEBUG

using System;
using static TLDAG.Libraries.Core.CodeGen.Parse;

namespace TLDAG.Libraries.Core.CodeGen
{
    public static partial class Grammar
    {
        private static readonly ProductionBuilder pbuilder = ProductionBuilder.Create(GrammarRexData);

        public static readonly Production GrammarGrammar = pbuilder.T("EOF").P("grammar", 1).Build();
    }
}

#endif