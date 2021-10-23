using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public class ProjectData
    {
        public int Id { get; }
        public string? File { get; }
        public List<ItemData> Items { get; }

        public ProjectData(int id, string? file, List<ItemData> items)
        {
            Id = id;
            File = file;
            Items = items;
        }
    }
}
