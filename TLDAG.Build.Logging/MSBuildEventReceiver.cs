using System;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventReceiver : IDisposable
    {
        private readonly MSBuildEventReceiverPipe pipe = new();

        ~MSBuildEventReceiver() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { pipe.Dispose(); }

        public MSBuildLoggerInfo GetSenderInfo()
            => MSBuildLoggerInfo.Create<MSBuildEventSender>(pipe.Handle);
    }
}
