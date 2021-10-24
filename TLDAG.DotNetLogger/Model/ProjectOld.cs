using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;

namespace TLDAG.DotNetLogger.Model
{
    public class ProjectOld : IComparable<ProjectOld>, IEquatable<ProjectOld>
    {
        [XmlAttribute("name")]
        public string Name
        {
            get => File is null ? "" : Path.GetFileNameWithoutExtension(File);

            set { }
        }

        [XmlElement("file")]
        public string? File { get; set; }

        [XmlAttribute("ids")]
        public string Ids
        {
            get => string.Join(";", ids);
            set { ids = new(value.Split(';').Select(p => int.Parse(p))); }
        }

        [XmlElement("items")]
        public List<ItemList> Items { get; set; } = new();

        private SortedSet<int> ids = new();

        public ProjectOld(string? file) { File = file; }
        public ProjectOld() : this(null) { }

        public bool HasId(int id) => ids.Contains(id);
        public void AddId(int id) { if (id >= 0) ids.Add(id); }

        public void AddOrReplace(List<ItemData> items) { items.ForEach(i => { GetItems(i.Type).AddOrReplace(i); }); }

        private ItemList GetItems(string type)
            => Items.Where(i => i.Type.Equals(type, StringComparison.Ordinal)).FirstOrDefault() ?? AddItems(type);

        private ItemList AddItems(string type)
        {
            ItemList items = new(type);

            Items.Add(items); Items.Sort();

            return items;
        }

        public int CompareTo(ProjectOld other) => StringComparer.Ordinal.Compare(Name, other.Name);

        public override int GetHashCode() => FileNameComparer.GetHashCode(File);
        public override bool Equals(object obj) => EqualsTo(obj as ProjectOld);
        public bool Equals(ProjectOld other) => EqualsTo(other);

        private bool EqualsTo(ProjectOld? other)
        {
            if (other is null) return false;
            if (File is null) return other.File is null;
            if (other.File is null) return false;
            return FileNameComparer.Compare(File, other.File) == 0;
        }

        public static bool operator ==(ProjectOld a, ProjectOld b) => a.EqualsTo(b);
        public static bool operator !=(ProjectOld a, ProjectOld b) => !a.EqualsTo(b);

        public static bool operator <(ProjectOld a, ProjectOld b) => a.CompareTo(b) < 0;
        public static bool operator >(ProjectOld a, ProjectOld b) => a.CompareTo(b) > 0;
        public static bool operator <=(ProjectOld a, ProjectOld b) => a.CompareTo(b) <= 0;
        public static bool operator >=(ProjectOld a, ProjectOld b) => a.CompareTo(b) >= 0;
    }
}
