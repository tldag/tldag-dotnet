using System;
using System.Collections.Generic;
using TLDAG.Libraries.Core.Algorithms;
using static TLDAG.Libraries.Core.Algorithms.BinarySearch;

namespace TLDAG.Libraries.Core.Collections
{
    public class Map<K, V>
        where K : notnull
    {
        public delegate bool EqualKeys(K a, K b);
        public delegate int SearchKey(K[] keys, K key, int count);

        private readonly EqualKeys equals;
        private readonly SearchKey search;

        private K[] keys = Array.Empty<K>();
        private V[] values = Array.Empty<V>();
        private int count = 0;

        public Map(EqualKeys equals, SearchKey search)
        { this.equals = equals; this.search = search; }

        public V? this[K key]
        {
            get
            {
                int pos = search(keys, key, count);

                if (pos >= count) return default;

                return equals(key, keys[pos]) ? values[pos] : default;
            }

            set { throw new NotImplementedException(); }
        }
    }

    public class SmartMap<K, V> : Map<K, V>
        where K : notnull, IEquatable<K>, IComparable<K>
    {
        private class Comparer : IComparer<K>
        {
            public int Compare(K? x, K? y)
            {
                throw new NotImplementedException();
            }
        }

        private static readonly Comparer comparer = new();

        private static readonly EqualKeys equalKeys
            = (a, b) => a.Equals(b);

        private static readonly SearchKey searchKey
            = (keys, key, count) => Search(keys, key, 0, count, comparer);

        public SmartMap() : base(equalKeys, searchKey) { }
    }
}
