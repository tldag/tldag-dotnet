using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Build.Analyze.Syntax;
using TLDAG.Core.Xml;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class SourceValuation
        {
            public SourceInfo Source { get; }
            public int Count { get; }

            public SourceValuation(SourceInfo source, int count) { Source = source; Count = count; }
        }

        public static SourceValuation GetValuation(SourceInfo source)
        {
            switch (source.Type)
            {
                case SourceType.Invalid: return new(source, 0);
                case SourceType.CSharp: return new(source, source.File.ParseCSharp().Valuation);
                case SourceType.Xml: return new(source, source.File.LoadXmlDocument().AllElements().Count());
            }

            return new(source, 0);
        }

        public static IEnumerable<SourceValuation> GetValuations(string path)
            => GetSources(path).Select(GetValuation);
    }
}
