using System;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlRestrictions
    {
        private readonly SortedSet<string> restrictedProperties = new(StringComparer.OrdinalIgnoreCase);
        private readonly SortedSet<string> restrictedMetadata = new(StringComparer.Ordinal);

        public void Initialize(DnlConfig config)
        {
            Clear();

            foreach (object key in Environment.GetEnvironmentVariables().Keys)
                restrictedProperties.Add(key.ToString());
            foreach (string key in config.AllowedProperties)
                restrictedProperties.Remove(key);

            foreach (string key in restrictedMetadataKeys)
                restrictedMetadata.Add(key);
            foreach (string key in config.AllowedMetadata)
                restrictedMetadata.Remove(key);
        }

        public void Shutdown() { Clear(); }

        private void Clear()
        {
            restrictedProperties.Clear();
            restrictedMetadata.Clear();
        }

        private static string[] restrictedMetadataKeys =
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
    }
}
