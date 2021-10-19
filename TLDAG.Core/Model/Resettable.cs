using System;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Model
{
    public abstract class Resettable : IDisposable
    {
        private bool disposed = false;

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (disposed) throw ObjectDisposed();

            Reset(); disposed = true;
        }

        protected abstract void Reset();
    }
}
