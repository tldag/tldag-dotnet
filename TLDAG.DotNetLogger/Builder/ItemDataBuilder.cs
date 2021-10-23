using Microsoft.Build.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.Conversion.Arguments;

namespace TLDAG.DotNetLogger.Builder
{
    public class ItemDataBuilder
    {
        private readonly List<ItemData> items = new();

        private ItemDataBuilder() { }

        public static ItemDataBuilder Create() => new();

        public ItemDataBuilder Add(IEnumerable taskItems)
            { items.AddRange(ToItems(taskItems).Select(CreateItemData)); return this; }

        public List<ItemData> Build() => items;

        private static ItemData CreateItemData(KeyValuePair<string, ITaskItem2> typeAndTaskItem)
        {
            string type = typeAndTaskItem.Key;
            ITaskItem2 taskItem = typeAndTaskItem.Value;
            string spec = taskItem.EvaluatedIncludeEscaped ?? string.Empty;

            return new(type, spec);
        }
    }
}
