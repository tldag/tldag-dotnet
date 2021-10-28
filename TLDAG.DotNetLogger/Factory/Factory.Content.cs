using System.Collections.Generic;
using System.Linq;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Factory
{
    public partial class DnlFactory
    {
        public List<DnlItem> CreateItems(ItemsAdapter source)
        {
            SortedSet<DnlItem> items = new();

            foreach (ItemAdapter i in source)
            {
                DnlItem item = CreateItem(i);

                items.Remove(item);
                items.Add(item);
            }

            return items.ToList();
        }

        public DnlItem CreateItem(ItemAdapter source)
        {
            DnlItem item = new(source.Type, source.Spec);

            return item;
        }
    }
}