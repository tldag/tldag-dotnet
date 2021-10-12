using System.Collections.Generic;
using TLDAG.Core.Collections;

namespace TLDAG.Core.Code.Internal
{
    internal static partial class Parse
    {
        internal class TerminalMap : UIntMap<Terminal> { }
        internal class ProductionMap : UIntMap<Production> { }
        internal class ElementMap : SmartMap<ElementKey, Element> { }
        internal class Hulls : SmartMap<Elements, Elements> { }
        internal class Firsts : SmartMap<Nodes, UIntSet> { }

        internal class TerminalDict : DenseImmUIntDict<Terminal>
        {
            public TerminalDict(IEnumerable<KeyValuePair<uint, Terminal>> keyValuePairs) : base(keyValuePairs) { }
        }

        internal class ProductionDict : DenseImmUIntDict<Production>
        {
            public ProductionDict(IEnumerable<KeyValuePair<uint, Production>> keyValuePairs) : base(keyValuePairs) { }
        }
    }
}