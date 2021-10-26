using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public interface IHasGlobals
    {
        public Properties? Globals { get; set; }

        public void SetGlobals(IEnumerable<StringEntry>? source);
    }

    public interface IHasProperties
    {
        public Properties? Properties { get; set; }

        public void SetProperties(IEnumerable<StringEntry>? source);
    }

    public interface IHasItems
    {
        public Items? Items { get; set; }

        public void SetItems(IEnumerable<Item>? source);
    }
}
