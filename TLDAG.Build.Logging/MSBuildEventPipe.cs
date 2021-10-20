using System;
using System.Collections.Generic;
using static TLDAG.Build.Logging.MSBuildEventModel;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventSenderPipe : IDisposable
    {
        public MSBuildEventSenderPipe(string handle, MSBuildEventSerializer serializer)
        {
            Console.WriteLine($"Created MSBuildEventSenderPipe({handle})");
        }

        ~MSBuildEventSenderPipe() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }

        private void Dispose(bool _)
        {
            Console.WriteLine("Disposing MSBuildEventSenderPipe");
        }

        public void Send(BuildData build)
        {
        }
    }

    public class MSBuildEventReceiverPipe : IDisposable
    {
        public string Handle { get => "123"; }

        ~MSBuildEventReceiverPipe() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }

        private void Dispose(bool _)
        {

        }
    }
}