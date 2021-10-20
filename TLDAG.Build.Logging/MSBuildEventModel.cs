using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ITaskItem2 = Microsoft.Build.Framework.ITaskItem2;

namespace TLDAG.Build.Logging
{
    public static class MSBuildEventModel
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static readonly StringComparison FileNameComparison
            = IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        private static SortedSet<string>? restrictedProperties = null;
        private static SortedSet<string> RestrictedProperties => restrictedProperties ??= GetRestrictedProperties();

        private static SortedSet<string> GetRestrictedProperties()
        {
            SortedSet<string> result = new();

            foreach (object key in Environment.GetEnvironmentVariables().Keys)
                result.Add(key.ToString());

            return result;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class StringEntry
        {
            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            public StringEntry(string key, string value) { Key = key; Value = value; }
            public StringEntry() : this("", "") { }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Properties
        {
            [JsonProperty("entries")]
            public List<StringEntry> Entries { get; set; } = new();

            public void AddEntries(IEnumerable<KeyValuePair<string, string>> entries)
            {
                foreach (KeyValuePair<string, string> entry in entries)
                    AddEntry(entry.Key, entry.Value);
            }

            public void AddEntry(string key, string value)
            {
                if (RestrictedProperties.Contains(key)) return;

                Remove(key);
                Entries.Add(new(key, value));
            }

            private void Remove(string key)
            {
                for (int i = 0, n = Entries.Count; i < n; ++i)
                {
                    if (Entries[i].Key.Equals(key))
                    {
                        Entries.RemoveAt(i);
                        Remove(key);
                        return;
                    }
                }
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Item
        {
            [JsonProperty("include")]
            public string Include { get; set; }

            public Item(string include) { Include = include; }
            public Item() : this("") { }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class ItemType
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("items")]
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

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class Project
        {
            [JsonProperty("file")]
            public string File { get; set; }

            [JsonProperty("globalProperties")]
            public Properties GlobalProperties { get; set; } = new();

            [JsonProperty("properties")]
            public Properties Properties { get; set; } = new();

            [JsonProperty("itemTypes")]
            public List<ItemType> ItemTypes { get; set; } = new();

            public Project(string file) { File = file; }
            public Project() : this("") { }

            public void AddGlobalProperties(IDictionary<string, string> properties)
                { GlobalProperties.AddEntries(properties); }

            public void AddGlobalProperties(IEnumerable<DictionaryEntry> properties)
            {
                foreach (DictionaryEntry entry in properties)
                    GlobalProperties.AddEntry(entry.Key.ToString(), entry.Value.ToString());
            }

            public void AddProperties(IEnumerable<DictionaryEntry> properties)
            {
                foreach (DictionaryEntry entry in properties)
                    Properties.AddEntry(entry.Key.ToString(), entry.Value.ToString());
            }

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
                ItemType? itemType = ItemTypes.Where(it => it.Type.Equals(key)).FirstOrDefault();

                if (itemType is null)
                {
                    ItemTypes.Add(itemType = new(key));
                }

                return itemType;
            }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class BuildResult
        {
            [JsonProperty("environment")]
            public Properties Environment { get; set; } = new();

            [JsonProperty("projects")]
            public List<Project> Projects { get; set; } = new();

            public void Clear()
            {
                Projects.Clear();
            }

            public Project GetProject(string file)
            {
                Project? project = Projects.Where(p => p.File.Equals(file, FileNameComparison)).FirstOrDefault();

                if (project is not null) return project;

                project = new(file);
                Projects.Add(project);

                Console.WriteLine($"Created project '{file}'");

                return project;
            }
        }
    }
}
