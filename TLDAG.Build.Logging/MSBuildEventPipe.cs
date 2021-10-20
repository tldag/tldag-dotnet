using System;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventSenderPipe : IDisposable
    {
        public MSBuildEventSenderPipe(string handle)
        {
            Console.WriteLine($"Created MSBuildEventSenderPipe({handle})");
        }

        ~MSBuildEventSenderPipe() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }

        private void Dispose(bool _)
        {
            Console.WriteLine("Disposing MSBuildEventSenderPipe");
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