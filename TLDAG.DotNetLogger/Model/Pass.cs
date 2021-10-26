using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.Model.Support.ItemsSupport;
using static TLDAG.DotNetLogger.Model.Support.PropertiesSupport;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    public class Pass : IHasGlobals, IHasProperties, IHasItems, IComparable<Pass>
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; }

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

        public void SetGlobals(IEnumerable<StringEntry>? source)
            { Globals = CreateProperties(source, FilterProperty); }

        public void SetProperties(IEnumerable<StringEntry>? source)
            { Properties = CreateProperties(source, FilterProperty); }

        public void SetItems(IEnumerable<Item>? source)
            { Items = CreateItems(source); }

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
