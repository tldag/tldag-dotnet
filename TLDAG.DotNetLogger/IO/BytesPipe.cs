using System;
using System.IO.Pipes;
using TLDAG.DotNetLogger.Threading;

namespace TLDAG.DotNetLogger.IO
{
    public class BytesPipe : IDisposable
    {
        public PipeStream Pipe { get; }
        public bool Compressed { get; }
        public Cancels Cancels { get; }

        public BytesPipe(PipeStream pipe, bool compressed)
        {
            Pipe = pipe;
            Compressed = compressed;
            Cancels = new();
        }

        ~BytesPipe() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { Cancels.Dispose(); }
    }
}
