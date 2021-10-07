using System;
using System.Collections;
using System.Collections.Generic;

namespace TLDAG.Core.Collections
{
    public abstract partial class ImmutableSet<T> : IReadOnlyList<T> where T : notnull
    {
        public int Count => values.Length;
        public T this[int index] => values[index];

        public IEnumerator<T> GetEnumerator() => CreateEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => CreateEnumerator();

        protected virtual IEnumerator<T> CreateEnumerator() => new Enumerator(this);

        protected class Enumerator : IEnumerator<T>
        {
            protected readonly ImmutableSet<T> set;
            protected int index = -1;

            public T Current => set[index];
            object IEnumerator.Current => set[index];

            public Enumerator(ImmutableSet<T> set) { this.set = set; }

            public void Dispose() { GC.SuppressFinalize(this); }
            public void Reset() { index = -1; }

            public bool MoveNext()
            {
                if (index + 1 < set.Count) { ++index; return true; }
                return false;
            }
        }
    }
}
