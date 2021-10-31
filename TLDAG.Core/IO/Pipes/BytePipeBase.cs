using System;
using System.IO.Pipes;
using TLDAG.Core.Threading;

namespace TLDAG.Core.IO.Pipes
{
    public class BytePipeBase : IDisposable
    {
        public PipeStream Pipe { get; }
        public Cancellation Cancellation { get; }

        public BytePipeBase(PipeStream pipe)
        {
            Pipe = pipe;
            Cancellation = new();
        }

        ~BytePipeBase() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { Cancellation.Dispose(); }
    }
}
