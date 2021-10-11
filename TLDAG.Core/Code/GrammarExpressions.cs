#if DEBUG

using TLDAG.Core.Collections;

namespace TLDAG.Core.Code
{
    public static partial class Grammar
    {
        public static readonly Rex.INode GrammarRexRoot = Rex.Empty();
        public static StringSet GrammarRexNames => StringSet.Empty();
    }
}

#endif