#if DEBUG

namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        private static readonly ParserBuilder pbuilder = new(GrammarRexNames);

        public static readonly Parse.IProduction GrammarDocument
            = pbuilder.P("grammar", 0).Build();
    }
}

#endif