using System;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;
using static TLDAG.DotNetLogger.Model.Support.SortedSetList;

namespace TLDAG.DotNetLogger.Model.Support
{
    public static class PropertiesSupport
    {
        public static bool FilterProperty(StringEntry entry)
            => !RestrictedPropertyKeys.Contains(entry.Key);

        public static Properties? CreateProperties(IEnumerable<StringEntry>? source, Func<StringEntry, bool>? filter = null)
        {
            List<StringEntry> entries = CreateSortedSetList(source, filter);

            return entries.Any() ? new(entries) : null;
        }

        public static Properties? CreateProperties(IDictionary<string, string>? source,
            Func<StringEntry, bool>? filter = null)
        {
            if (source is null || !source.Any()) return null;

            IEnumerable<StringEntry> entries = source
                .Where(kvp => kvp.Key is not null && !string.IsNullOrWhiteSpace(kvp.Key))
                .Select(kvp => new StringEntry(kvp.Key, kvp.Value));

            return CreateProperties(entries, filter);
        }
    }
}
