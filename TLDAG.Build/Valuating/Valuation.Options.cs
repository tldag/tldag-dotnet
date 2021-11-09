using System.Linq;
using TLDAG.Core.Collections;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public static readonly StringSet SupportedProjectExtensions = new(".csproj");
        public static readonly StringSet SupportedProjectPatterns = new(SupportedProjectExtensions.Select(ext => $"*{ext}"));

        public class Options
        {
            private InputFactory? inputFactory = null;
            public InputFactory InputFactory { get => inputFactory ??= new(this); set { inputFactory = value; } }

            private ProjectFactory? projectFactory = null;
            public ProjectFactory ProjectFactory { get => projectFactory ??= new(this); set { projectFactory = value; } }

            private SourceFactory? sourceFactory = null;
            public SourceFactory SourceFactory { get => sourceFactory ??= new(this); set { sourceFactory = value; } }

            private ValuatorFactory? valuatorFactory = null;
            public ValuatorFactory ValuatorFactory { get => valuatorFactory ??= new(this); set { valuatorFactory = value; } }

            private SerializerFactory? serializerFactory = null;
            public SerializerFactory SerializerFactory { get => serializerFactory ??= new(this); set { serializerFactory = value; } }

            public string Currency { get; set; } = "CHF";
            public int Multiplier { get; set; } = 8;
        }

        public class Factory
        {
            public Options Options { get; }

            public Factory(Options options) { Options = options; }
        }
    }
}