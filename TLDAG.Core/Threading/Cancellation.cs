using System;
using System.Collections.Generic;
using System.Threading;

namespace TLDAG.Core.Threading
{
    public class Cancellation : IDisposable
    {
        private readonly List<CancellationTokenSource> sources = new();
        private CancellationTokenSource? noDelaySource = null;

        public CancellationTokenSource NoDelaySource { get => noDelaySource ??= new(); }
        public CancellationToken NoDelayToken { get => NoDelaySource.Token; }

        ~Cancellation() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { Cancel(false); }

        public CancellationToken Token(TimeSpan? delay = null)
        {
            if (delay is null) return NoDelayToken;

            CancellationTokenSource source = new(delay.Value);
            CancellationToken token = source.Token;

            sources.Add(source);

            return token;
        }

        public void Cancel(bool throwOnFirstException)
        {
            sources.ForEach(source => { source.Cancel(throwOnFirstException); });
            noDelaySource?.Cancel(throwOnFirstException);
        }
    }
}
