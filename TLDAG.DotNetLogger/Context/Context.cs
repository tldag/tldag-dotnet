using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TLDAG.DotNetLogger.Context
{
    public class DnlContext
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        private readonly SortedSet<string> restrictedProperties = new();
        private readonly SortedSet<string> restrictedMetadata = new();

        public DnlContext(DnlConfig config)
        {
            foreach (string name in Restrictions.RestrictedProperties) restrictedProperties.Add(name);
            foreach (string name in config.AllowedProperties) restrictedProperties.Remove(name);

            foreach (string name in Restrictions.RestrictedMetadata) restrictedMetadata.Add(name);
            foreach (string name in config.AllowedMetadata) restrictedMetadata.Remove(name);
        }
    }
}
