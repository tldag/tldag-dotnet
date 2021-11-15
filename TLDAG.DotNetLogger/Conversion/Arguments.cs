using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.DotNetLogger.Conversion
{
    public static class Arguments
    {
        public static IEnumerable<KeyValuePair<string, ITaskItem2>> ToItems(IEnumerable? items)
        {
            if (items is IEnumerable<DictionaryEntry> entries)
            {
                foreach (DictionaryEntry entry in entries)
                {
                    if (entry.Key is string key && entry.Value is ITaskItem2 item)
                        yield return new KeyValuePair<string, ITaskItem2>(key, item);
                }
            }
        }
    }
}
