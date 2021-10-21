using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;
using ITaskItem2 = Microsoft.Build.Framework.ITaskItem2;

namespace TLDAG.Build.Logging
{
    public static class MSBuildEventModel
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static readonly StringComparison FileNameComparison
            = IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        public static readonly StringComparer FileNameComparer
            = IsWindows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        private static SortedSet<string>? restrictedProperties = null;
        private static SortedSet<string> RestrictedProperties => restrictedProperties ??= GetRestrictedProperties();

        private static SortedSet<string> GetRestrictedProperties()
        {
            SortedSet<string> result = new(StringComparer.OrdinalIgnoreCase);

            foreach (object key in Environment.GetEnvironmentVariables().Keys)
                result.Add(key.ToString());

            return result;
        }

        public class StringEntry
        {
            [XmlAttribute("key")]
            public string Key { get; set; }

            [XmlAttribute("value")]
            public string Value { get; set; }

            public StringEntry(string key, string value) { Key = key; Value = value; }
            public StringEntry() : this("", "") { }
        }

        public class Properties
        {
            [XmlElement("entry")]
            public List<StringEntry> Entries { get; set; } = new();

            [XmlIgnore]
            public Dictionary<string, string> Map
            {
                get => Entries.ToDictionary(e => e.Key, e => e.Value);
                private set
                {
                    IEnumerable<StringEntry> entries = value
                        .OrderBy(kvp => kvp.Key, StringComparer.Ordinal)
                        .Select(kvp => new StringEntry(kvp.Key, kvp.Value));

                    Entries = new(entries);
                }
            }

            public string? this[string key]
            {
                get => Entries.Where(e => e.Key.Equals(key)).FirstOrDefault()?.Value;
                set
                {
                    Dictionary<string, string> map = Map;

                    map.Remove(key);

                    if (value is not null && !RestrictedProperties.Contains(key))
                        map[key] = value;

                    Map = map;
                }
            }

            public void Clear() { Entries.Clear(); }

            public void AddOrReplace(Dictionary<string, string> input)
            {
                Dictionary<string, string> map = Map;

                foreach (KeyValuePair<string, string> kvp in input)
                {
                    if (RestrictedProperties.Contains(kvp.Key)) continue;

                    map.Remove(kvp.Key);
                    map[kvp.Key] = kvp.Value;
                }

                Map = map;
            }
        }

        public class Item
        {
            public string Include { get; set; }

            public Item(string include) { Include = include; }
            public Item() : this("") { }
        }

        public class ItemType
        {
            public string Type { get; set; }

            public List<Item> Items { get; set; } = new();

            public ItemType(string type) { Type = type; }
            public ItemType() : this("") { }

            public void AddItem(ITaskItem2 taskItem)
            {
                string include = taskItem.EvaluatedIncludeEscaped;
                Item item = new(include);

                Remove(include);
                Items.Add(item);
            }

            private void Remove(string include)
            {
                for (int i = 0, n = Items.Count; i < n; ++i)
                {
                    if (Items[i].Include.Equals(include))
                    {
                        Items.RemoveAt(i);
                        Remove(include);
                        return;
                    }
                }
            }
        }

        public class Project
        {
            [XmlAttribute("name")]
            public string Name
            {
                get => Properties["ProjectName"] ?? Path.GetFileNameWithoutExtension(File);
                set { Properties["ProjectName"] = value; }
            }

            [XmlElement("file")]
            public string File { get; set; }

            [XmlElement("globals")]
            public Properties Globals { get; set; } = new();

            [XmlElement("properties")]
            public Properties Properties { get; set; } = new();

            [XmlElement("types")]
            public List<ItemType> Types { get; set; } = new();

            public Project(string file) { File = file; }
            public Project() : this("") { }

            public void AddItems(IEnumerable<DictionaryEntry> items)
            {
                foreach (DictionaryEntry item in items)
                {
                    if (item.Value is ITaskItem2 taskItem)
                        GetItemType(item.Key.ToString()).AddItem(taskItem);
                }
            }

            private ItemType GetItemType(string key)
            {
                ItemType? itemType = Types.Where(it => it.Type.Equals(key)).FirstOrDefault();

                if (itemType is null)
                {
                    Types.Add(itemType = new(key));
                }

                return itemType;
            }
        }

        public class ProjectMap
        {
            public List<Project> Projects
            {
                get => byFile.Values.OrderBy(p => p.Name).ToList();
                set
                {
                    Clear();

                    foreach (Project project in value)
                        byFile[project.File] = project;
                }
            }

            private readonly Dictionary<string, Project> byFile = new(FileNameComparer);
            private readonly Dictionary<int, Project> byId = new();

            public void Clear() { byFile.Clear(); byId.Clear(); }

            public Project GetProject(int id, string file)
            {
                if (byId.TryGetValue(id, out Project? project)) return project;
                byFile.TryGetValue(file, out project);

                if (project is null)
                    byFile[file] = project = new(file);

                if (id >= 0) byId[id] = project;

                Console.WriteLine($"registered {id} = {project.Name}");

                return project;
            }
        }

        [XmlRoot("build")]
        public class BuildResult
        {
            [XmlElement("environment")]
            public Properties Environment { get; set; } = new();

            [XmlElement("project")]
            public List<Project> Projects
            {
                get => projects.Projects;
                set { projects.Projects = value; }
            }

            private ProjectMap projects = new();

            public void Clear()
            {
                Environment.Clear();
                projects.Clear();
            }

            public void AddBuildData(MSBuildBuildData data)
            {
                Environment.AddOrReplace(data.Environment);
            }

            public void AddProjectData(MSBuildProjectData data)
            {
                Project project = projects.GetProject(data.Id, data.File);

                project.Globals.AddOrReplace(data.Globals);
                project.Properties.AddOrReplace(data.Properties);
            }

            public static XmlSerializer Serializer { get; } = new(typeof(BuildResult));
            private static XmlWriterSettings Settings { get; } = new() { Indent = false };

            public string Serialize(XmlWriterSettings? settings = null)
            {
                settings ??= Settings;

                using StringWriter stringWriter = new();
                using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

                Serializer.Serialize(xmlWriter, this);

                return stringWriter.ToString();
            }

            public static BuildResult Deserialize(string xml)
            {
                using StringReader reader = new(xml);

                return (BuildResult)Serializer.Deserialize(reader);
            }
        }
    }
}
