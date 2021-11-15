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

        private static SortedSet<string>? restrictedPropertyKeys = null;
        public static SortedSet<string> RestrictedPropertyKeys => restrictedPropertyKeys ??= GetRestrictedPropertyKeys();

        private static SortedSet<string> GetRestrictedPropertyKeys()
        {
            SortedSet<string> result = new(StringComparer.OrdinalIgnoreCase);

            foreach (object key in Environment.GetEnvironmentVariables().Keys)
            {
                if (key is string keyString)
                    result.Add(keyString);
            }
                

            return result;
        }

        private static SortedSet<string>? restrictedMetadataKeys = null;
        public static SortedSet<string> RestrictedMetadataKeys => restrictedMetadataKeys ??= GetRestrictedMetadataKeys();

        private static SortedSet<string> GetRestrictedMetadataKeys()
        {
            string[] keys = 
            {
                "AccessedTime",
                "CreatedTime",
                "DefiningProjectDirectory",
                "DefiningProjectExtension",
                "DefiningProjectFullPath",
                "DefiningProjectName",
                "Directory",
                "Extension",
                "Filename",
                "FullPath",
                "Identity",
                "ModifiedTime",
                "RecursiveDir",
                "RelativeDir",
                "RootDir"
            };

            return new(keys);
        }
    }
}
