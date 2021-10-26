using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Pass : IComparable<Pass>
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

        [XmlElement("info")]
        public List<string>? Infos { get; set; } = null;

        [XmlElement("globals")]
        public Properties? Globals { get; set; } = null;

        [XmlElement("properties")]
        public Properties? Properties { get; set; } = null;

        [XmlElement("items")]
        public Items? Items { get; set; } = null;

        [XmlElement("targets")]
        public Targets? Targets { get; set; } = null;

        public Pass(int id) { Id = id; }
        public Pass() : this(-1) { }

        public int CompareTo(Pass other) => Id.CompareTo(other.Id);

        public void SetGlobals(IEnumerable<StringEntry> source)
        {
            if (!source.Any()) return;

            Globals ??= new();
            Globals.Set(source);
        }

        public void SetProperties(IEnumerable<StringEntry> source)
        {
            if (!source.Any()) return;

            Properties ??= new();
            Properties.Set(source);
        }

        public void AddOrReplaceItems(IEnumerable<Item> source)
        {
            if (!source.Any()) return;

            Items ??= new();
            Items.AddOrReplace(source);
        }

        public Target? GetTarget(int id) => Targets?.Get(id);

        public Target AddTarget(string? name, int id)
        {
            if (name is null || string.IsNullOrWhiteSpace(name))
                return new();

            Targets ??= new();

            return Targets.Add(name, id);
        }
    }
}
