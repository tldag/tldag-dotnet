using System;
using System.Collections.Generic;
using System.Linq;

namespace TLDAG.DotNetLogger.Model.Support
{
    public static class SortedSetList
    {
        public static List<T> CreateSortedSetList<T>(IEnumerable<T>? source, Func<T, bool>? filter = null)
            where T : notnull, IComparable<T>, IEquatable<T>
        {
            if (source is null) return new();

            IEnumerable<T> filtered = filter is null ? source : source.Where(filter);
            SortedSet<T> set = new();

            foreach (T entry in filtered)
            {
                set.Remove(entry);
                set.Add(entry);
            }

            return set.ToList();
        }
    }
}
