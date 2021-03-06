using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static TLDAG.Core.Algorithms.Arrays;
using static TLDAG.Core.Algorithms.BinarySearch;
using static TLDAG.Core.Delegates;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Primitives;
using static TLDAG.Core.Strings;

namespace TLDAG.Core.Collections
{
    public class MapEnumerator<K, V> : IEnumerator<KeyValuePair<K, V>>
        where K : notnull
    {
        public KeyValuePair<K, V> Current => GetCurrent();
        object IEnumerator.Current => GetCurrent();

        private int count;
        private List<K> keys;
        private List<V> values;
        private int index;

        public MapEnumerator(IEnumerable<K> keys, IEnumerable<V> values)
        {
            count = Math.Min(keys.Count(), values.Count());
            this.keys = new(keys.Take(count));
            this.values = new(values.Take(count));
            index = -1;
        }

        public void Dispose() { GC.SuppressFinalize(this); }
        public void Reset() { index = -1; }
        public bool MoveNext() { if (index < count) ++index; return index < count; }

        private KeyValuePair<K, V> GetCurrent() => new(keys[index], values[index]);
    }

    public class Map<K, V> : IEnumerable<KeyValuePair<K, V>>
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

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => new MapEnumerator<K, V>(Keys, Values);
        IEnumerator IEnumerable.GetEnumerator() => new MapEnumerator<K, V>(Keys, Values);

        private V? InsertValue(K key, V value)
        {
            int pos = SearchKey(key);

            if (pos == count) return InsertValueAt(pos, key, value);
            if (EqualKeys(key, keys[pos])) return ReplaceValueAt(pos, value);
            return InsertValueAt(pos, key, value);
        }

        private V ReplaceValueAt(int pos, V value) { V old = values[pos]; values[pos] = value; return old; }

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
        public IntMap(IEnumerable<KeyValuePair<int, V>> values) : base(IntCompare) { throw NotYetImplemented(); }

        protected override int SearchKey(int key) => Search(keys, key, 0, count);
        protected override bool EqualKeys(int a, int b) => a == b;
    }

    public class UIntMap<V> : Map<uint, V>
    {
        public UIntMap() : base(UIntCompare) { }
        public UIntMap(IEnumerable<KeyValuePair<uint, V>> values) : base(UIntCompare) { throw NotYetImplemented(); }

        protected override int SearchKey(uint key) => Search(keys, key, 0, count);
        protected override bool EqualKeys(uint a, uint b) => a == b;
    }

    public class StringMap<V> : Map<string, V>
    {
        public StringMap(IComparer<string> comparer) : this(comparer.ToCompare()) { }
        public StringMap(Compare<string>? compare = null) : base(compare ??= CompareOrdinal) { }
    }
}
