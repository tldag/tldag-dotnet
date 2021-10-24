using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TLDAG.DotNetLogger.Threading
{
    public class Cancels : IDisposable
    {
        private readonly List<CancellationTokenSource> sources = new();
        private CancellationTokenSource? noDelaySource = null;

        public CancellationTokenSource NoDelaySource { get => noDelaySource ??= new(); }
        public CancellationToken NoDelayToken { get => NoDelaySource.Token; }

        ~Cancels() { Dispose(false); }
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
