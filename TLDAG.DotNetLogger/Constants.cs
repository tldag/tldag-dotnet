using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TLDAG.DotNetLogger
{
    public static class DotNetLoggerConstants
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static readonly StringComparison FileNameComparison
            = IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        public static readonly StringComparer FileNameComparer
            = IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        public static readonly StringComparer EnvNameComparer
            = IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        private static SortedSet<string>? restrictedProperties = null;
        internal static SortedSet<string> RestrictedProperties => restrictedProperties ??= GetRestrictedProperties();

        private static SortedSet<string> GetRestrictedProperties()
        {
            SortedSet<string> result = new(StringComparer.OrdinalIgnoreCase);

            foreach (object key in Environment.GetEnvironmentVariables().Keys)
                result.Add(key.ToString());

            return result;
        }
    }
}
