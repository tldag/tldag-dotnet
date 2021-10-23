using System;
using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Algorithm
{
    public static class Algorithms
    {
        public static void Merge<K, V>(IDictionary<K, V> source, IDictionary<K, V> target)
        {
            foreach (KeyValuePair<K, V> kvp in source)
            {
                target.Remove(kvp.Key);
                target[kvp.Key] = kvp.Value;
            }
        }

        public static void RemoveWhere<T>(List<T> list, Func<T, bool> predicate)
        {
            for (int i = 0, n = list.Count; i < n; ++i)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    RemoveWhere(list, predicate);
                    return;
                }
            }
        }
    }
}
