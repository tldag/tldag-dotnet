using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

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
            return source.Type switch
            {
                SourceType.Invalid => new(source, 0),
                SourceType.CSharp => new(source, CSharpValuator.Instance.Valuate(source.File)),
                SourceType.Xml => new(source, XmlValuator.Instance.Valuate(source.File)),
                _ => new(source, 0),
            };
        }

        public static IEnumerable<SourceValuation> GetValuations(string path)
            => GetSources(path).Select(GetValuation);
    }
}
