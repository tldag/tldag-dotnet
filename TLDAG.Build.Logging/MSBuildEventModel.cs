using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TLDAG.Build.Logging
{
    public static class MSBuildEventModel
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static readonly IEqualityComparer<string> FileNameComparer
            = IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Project
        {
            public void AddGlobalProperties(IDictionary<string, string>? properties)
            {
            }

            public void AddGlobalProperties(IEnumerable<DictionaryEntry> properties)
            {

            }

            public void AddProperties(IEnumerable<DictionaryEntry> properties)
            {
            }

            public void AddItems(IEnumerable<DictionaryEntry> items)
            {
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class BuildResult
        {
            private Dictionary<string, Project> projects = new(FileNameComparer);

            public void Clear()
            {
                projects.Clear();
            }

            public void SetEnvironment(IDictionary<string, string>? environment)
            {
            }

            public Project GetProject(string file)
            {
                if (projects.TryGetValue(file, out Project? project)) return project;

                project = new();
                projects[file] = project;

                Console.WriteLine($"Created project '{file}'");

                return project;
            }
        }
    }
}
