using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;
using static TLDAG.DotNetLogger.Algorithm.Algorithms;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Properties
    {
        [XmlElement("entry")]
        public List<StringEntry> Entries { get; set; } = new();

        public void AddOrReplace(IDictionary<string, string> source)
        {
            foreach (KeyValuePair<string, string> kvp in source)
                AddOrReplace(kvp.Key, kvp.Value);
        }

        public void AddOrReplace(string key, string? value)
        {
            Remove(key);
            
            if (value is not null && !RestrictedProperties.Contains(key))
            {
                Entries.Add(new(key, value));
                Entries.Sort();
            }
        }

        public void Remove(string key) => RemoveWhere(Entries, e => e.Key.Equals(key));
    }
}
