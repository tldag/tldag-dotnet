using System.Collections.Generic;
using TLDAG.Build.Logging;

namespace TLDAG.Build.DotNet
{
    public class DotNetOptions
    {
        public List<MSBuildLoggerInfo> Loggers = new();
    }
}
