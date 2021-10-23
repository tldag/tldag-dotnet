using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.DotNetLogger.Conversion
{
    public static class Arguments
    {
        private static readonly IEnumerable<KeyValuePair<string, ITaskItem2>> NoItems
            = Array.Empty<KeyValuePair<string, ITaskItem2>>();

        public static IEnumerable<KeyValuePair<string, ITaskItem2>> ToItems(IEnumerable? items)
        {
            if (items is not IEnumerable<DictionaryEntry> entries) return NoItems;

            return entries
                .Where(e => e.Value is ITaskItem2)
                .Select(e => new KeyValuePair<string, ITaskItem2>(e.Key.ToString(), (ITaskItem2)e.Value));
        }
    }
}
