using System;
using System.IO;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventReceiver : IDisposable
    {
        private readonly MSBuildEventReceiverPipe pipe = new();

        ~MSBuildEventReceiver() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { pipe.Dispose(); }

        public string GetSenderLogger()
        {
            Type type = typeof(MSBuildEventSender);

            return $"{type.FullName},\"{type.Assembly.Location}\";{pipe.Handle}";
        }
    }
}
