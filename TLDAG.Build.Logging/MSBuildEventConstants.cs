using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TLDAG.Build.Logging
{
    public static class MSBuildEventConstants
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static readonly IEqualityComparer<string> FileNameComparer
            = IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    }
}
