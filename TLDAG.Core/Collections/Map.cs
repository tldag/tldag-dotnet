using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Algorithms;
using static TLDAG.Core.Algorithms.Arrays;
using static TLDAG.Core.Algorithms.BinarySearch;

namespace TLDAG.Core.Collections
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

        public int Count => count;
        public IReadOnlyList<K> Keys => keys.Take(count).ToList();
        public IReadOnlyList<V> Values => values.Take(count).ToList();

        public Map(EqualKeys equals, SearchKey search) { this.equals = equals; this.search = search; }

        public V? this[K key]
        {
            get => GetValue(key);
            set { SetValue(key, value); }
        }

        public V? GetValue(K key)
        {
            int pos = search(keys, key, count);

            if (pos == count) return default;

            return equals(key, keys[pos]) ? values[pos] : default;
        }

        public V? SetValue(K key, V? value)
            => value == null ? RemoveKey(key) : InsertValue(key, value);

        public bool ContainsKey(K key)
        {
            int pos = search(keys, key, count);

            return pos != count && equals(key, keys[pos]);
        }

        public V? RemoveKey(K key)
        {
            throw new NotImplementedException();
        }

        private V? InsertValue(K key, V value)
        {
            int pos = search(keys, key, count);

            if (pos == count) return InsertValueAt(pos, key, value);
            if (equals(key, keys[pos])) return ReplaceValueAt(pos, value);
            return InsertValueAt(pos, key, value);
        }

        private V ReplaceValueAt(int pos, V value)
        {
            throw new NotImplementedException();
        }

        private V? InsertValueAt(int pos, K key, V value)
        {
            EnsureCapacity(count + 1);

            if (pos < count)
            {
                Move(keys, pos, pos + 1, count - pos);
                Move(values, pos, pos + 1, count - pos);
            }

            keys[pos] = key;
            values[pos] = value;
            ++count;

            return default;
        }

        private void EnsureCapacity(int required)
        {
            if (keys.Length >= required) return;

            int newCapacity = ((required + 15) / 16) * 16;

            keys = Resize(keys, newCapacity);
            values = Resize(values, newCapacity);
        }
    }

    public class SmartMap<K, V> : Map<K, V>
        where K : notnull, IEquatable<K>, IComparable<K>
    {
        private static readonly Compare<K> compare = (a, b) => a.CompareTo(b);
        private static readonly EqualKeys equalKeys = (a, b) => a.Equals(b);
        private static readonly SearchKey searchKey = (keys, key, count) => Search(keys, key, 0, count, compare);

        public SmartMap() : base(equalKeys, searchKey) { }
    }

    public class IntMap<V> : Map<int, V>
    {
        private static readonly EqualKeys equalKeys = (a, b) => a == b;
        private static readonly SearchKey searchKey = (keys, key, count) => Search(keys, key, 0, count);

        public IntMap() : base(equalKeys, searchKey) { }
    }
}
