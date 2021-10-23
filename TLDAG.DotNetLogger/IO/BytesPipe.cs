using System;
using System.IO.Pipes;
using System.Threading;
using static TLDAG.DotNetLogger.Conversion.Cancellation;

namespace TLDAG.DotNetLogger.IO
{
    public class BytesPipe
    {
        public PipeStream Pipe { get; }
        public bool Compressed { get; }
        public CancellationTokenSource Cancel { get; }

        public BytesPipe(PipeStream pipe, bool compressed, TimeSpan? timeout)
        {
            Pipe = pipe;
            Compressed = compressed;
            Cancel = CreateCancelSource(timeout);
        }
    }
}
