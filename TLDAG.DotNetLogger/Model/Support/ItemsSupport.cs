using System.Collections.Generic;
using System.Linq;
using static TLDAG.DotNetLogger.Model.Support.SortedSetList;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;

namespace TLDAG.DotNetLogger.Model.Support
{
    public static class ItemsSupport
    {
        public static bool FilterMetadata(StringEntry entry)
            => !RestrictedMetadataKeys.Contains(entry.Key);

        public static Items? CreateItems(IEnumerable<Item>? source)
        {
            List<Item> entries = CreateSortedSetList(source);

            return entries.Any() ? new(entries) : null;
        }
    }
}
