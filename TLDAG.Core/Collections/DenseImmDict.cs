using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static TLDAG.Core.Errors;

namespace TLDAG.Core.Collections
{
    public class DenseImmUIntDict<V> : IReadOnlyDictionary<uint, V>, IEquatable<DenseImmUIntDict<V>>
    {
        public V this[uint key] => throw NotYetImplemented();

        private List<uint>? keys = null;
        public IEnumerable<uint> Keys => keys ??= ComputeKeys();

        public IEnumerable<V> Values => throw NotYetImplemented();
        public int Count => throw NotYetImplemented();

        private V[] values;

        public DenseImmUIntDict(IEnumerable<KeyValuePair<uint, V>> keyValuePairs)
        {
            values = new V[Size(keyValuePairs)];

            Fill(keyValuePairs, values);
        }

        public bool ContainsKey(uint key)
        {
            throw NotYetImplemented();
        }

        public IEnumerator<KeyValuePair<uint, V>> GetEnumerator()
        {
            throw NotYetImplemented();
        }

#if NET5_0_OR_GREATER
        public bool TryGetValue(uint key, [MaybeNullWhen(false)] out V value)
#else
        public bool TryGetValue(uint key, out V value)
#endif
        {
            throw NotYetImplemented();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw NotYetImplemented();
        }

        public override int GetHashCode()
        {
            throw NotYetImplemented();
        }

        public override bool Equals(object? obj)
        {
            throw NotYetImplemented();
        }

        public bool Equals(DenseImmUIntDict<V>? other)
        {
            throw NotYetImplemented();
        }

        private List<uint> ComputeKeys()
        {
            List<uint> result = new();

            for (int i = 0, n = values.Length; i < n; ++i)
            {
                if (values[i] is not null) result.Add((uint)i);
            }

            return result;
        }

        private static int Size(IEnumerable<KeyValuePair<uint, V>> keyValuePairs)
        {
            if (keyValuePairs.Count() == 0) return 0;

            uint max = keyValuePairs.Max(kvp => kvp.Key);

            if (max >= int.MaxValue)
                throw OutOfRange("keyValuePairs", max, $"must not exceed {int.MaxValue - 1}");

            return (int)(max + 1);
        }

        private static void Fill(IEnumerable<KeyValuePair<uint, V>> keyValuePairs, V[] values)
        {
            foreach (KeyValuePair<uint, V> kvp in keyValuePairs)
            {
                values[kvp.Key] = kvp.Value;
            }
        }
    }
}