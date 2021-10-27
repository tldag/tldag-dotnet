using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Context
{
    public static class Restrictions
    {
        private static SortedSet<string>? restrictedProperties = null;
        public static SortedSet<string> RestrictedProperties => restrictedProperties ??= GetRestrictedProperties();

        private static SortedSet<string> GetRestrictedProperties()
        {
            SortedSet<string> result = new(StringComparer.OrdinalIgnoreCase);

            foreach (object key in Environment.GetEnvironmentVariables().Keys)
                result.Add(key.ToString());

            return result;
        }

        private static SortedSet<string>? restrictedMetadata = null;
        public static SortedSet<string> RestrictedMetadata => restrictedMetadata ??= GetRestrictedMetadata();

        private static SortedSet<string> GetRestrictedMetadata()
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
