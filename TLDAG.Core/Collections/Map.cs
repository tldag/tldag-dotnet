using System;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Core.Algorithms;
using static TLDAG.Core.Algorithms.Arrays;
using static TLDAG.Core.Algorithms.BinarySearch;
using static TLDAG.Core.Exceptions;
using static TLDAG.Core.Delegates;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.Collections
{
    public class Map<K, V>
        where K : notnull
    {
        protected readonly Compare<K> compare;

        protected K[] keys = Array.Empty<K>();
        protected V[] values = Array.Empty<V>();
        protected int count = 0;

        public int Count => count;
        public IReadOnlyList<K> Keys => keys.Take(count).ToList();
        public IReadOnlyList<V> Values => values.Take(count).ToList();

        public Map(Compare<K> compare) { this.compare = compare; }

        public V? this[K key] { get => GetValue(key); set { SetValue(key, value); } }

        public V? GetValue(K key)
            { int pos = SearchKey(key); return pos < count && EqualKeys(key, keys[pos]) ? values[pos] : default; }

        public V? SetValue(K key, V? value)
            => value == null ? RemoveKey(key) : InsertValue(key, value);

        public bool ContainsKey(K key) { int pos = SearchKey(key); return pos < count && EqualKeys(key, keys[pos]); }

        public V? RemoveKey(K key)
        {
            throw NotYetImplemented();
        }

        private V? InsertValue(K key, V value)
        {
            int pos = SearchKey(key);

            if (pos == count) return InsertValueAt(pos, key, value);
            if (EqualKeys(key, keys[pos])) return ReplaceValueAt(pos, value);
            return InsertValueAt(pos, key, value);
        }

        private V ReplaceValueAt(int pos, V value)
        {
            throw NotYetImplemented();
        }

        private V? InsertValueAt(int pos, K key, V value)
        {
            EnsureCapacity(count + 1);

            if (pos < count)
            {
                Move(keys, pos, pos + 1, count - pos);
                Move(values, pos, pos + 1, count - pos);
            }

            keys[pos] = key; values[pos] = value; ++count;

            return default;
        }

        private void EnsureCapacity(int required)
        {
            if (keys.Length >= required) return;

            int newCapacity = ((required + 15) / 16) * 16;

            keys = Resize(keys, newCapacity);
            values = Resize(values, newCapacity);
        }

        protected virtual int SearchKey(K key) => Search(keys, key, 0, count, compare);
        protected virtual bool EqualKeys(K a, K b) => compare(a, b) == 0;
    }

    public class SmartMap<K, V> : Map<K, V>
        where K : notnull, IEquatable<K>, IComparable<K>
    {
        public SmartMap() : base(GetCompare<K>()) { }
    }

    public class IntMap<V> : Map<int, V>
    {
        public IntMap() : base(IntCompare) { }

        protected override int SearchKey(int key) => Search(keys, key, 0, count);
        protected override bool EqualKeys(int a, int b) => a == b;
    }
}
