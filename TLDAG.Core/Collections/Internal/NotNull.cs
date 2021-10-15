using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core.Collections.Internal
{
    internal class NotNullEnumerator<T> : IEnumerator<T> where T : notnull
    {
        private T? current = default;
        public T Current => Contract.State.NotNull(current, "Enumerator call out of sequence.");
        object IEnumerator.Current => Contract.State.NotNull(current, "Enumerator call out of sequence.");

        private readonly IEnumerable<T?> source;
        private IEnumerator<T?> input;
        private bool done = false;

        public NotNullEnumerator(IEnumerable<T?> source) { this.source = source; input = source.GetEnumerator(); }

        public void Dispose() { }
        public void Reset() { current = default; input = source.GetEnumerator(); done = false; }

        public bool MoveNext()
        {
            if (done) return false;

            while(true)
            {
                if (!input.MoveNext()) { done = true; return false; }
                if ((current = input.Current) is not null) return true;
            }
        }
    }

    public class NotNulls<T> : IEnumerable<T> where T : notnull
    {
        private readonly IEnumerable<T?> source;

        public NotNulls(IEnumerable<T?> source) { this.source = source; }

        public IEnumerator<T> GetEnumerator() => new NotNullEnumerator<T>(source);
        IEnumerator IEnumerable.GetEnumerator() => new NotNullEnumerator<T>(source);
    }
}
