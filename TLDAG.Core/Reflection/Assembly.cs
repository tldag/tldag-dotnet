using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TLDAG.Core.Collections;

namespace TLDAG.Core.Reflection
{
    public static class AssemblyExtensions
    {
        public static string Name(this Assembly assembly)
            => assembly.GetName()?.Name ?? "Unknown";

        public static string Configuration(this Assembly assembly)
        {
            AssemblyConfigurationAttribute? attribute = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>();

            return attribute?.Configuration ?? "Unknown";
        }

        public static IEnumerable<string> GetMetadataValues(this Assembly assembly, string key)
        {
            return assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Where(a => key.Equals(a.Key))
                .Select(a => a.Value).NotNull();
        }
    }
}