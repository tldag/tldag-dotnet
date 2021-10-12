using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Collections
{
    public class DenseImmUIntDict<V> : IReadOnlyDictionary<uint, V>, IEquatable<DenseImmUIntDict<V>>
    {
        public V this[uint key] => throw NotYetImplemented();
        public IEnumerable<uint> Keys => throw NotYetImplemented();
        public IEnumerable<V> Values => throw NotYetImplemented();
        public int Count => throw NotYetImplemented();

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
    }
}