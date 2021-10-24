using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using static TLDAG.DotNetLogger.Algorithm.Deflater;
using static TLDAG.DotNetLogger.Conversion.Primitives;

namespace TLDAG.DotNetLogger.IO
{
    public class BytesPipeSentEventArgs : EventArgs
    {
        public int Count { get; }
        public bool Compressed { get; }

        public BytesPipeSentEventArgs(int count, bool compressed) { Count = count; Compressed = compressed; }
    }

    public delegate void BytesPipeSentHandler(BytesPipeSender sender, BytesPipeSentEventArgs args);

    public class BytesPipeSender : BytesPipe
    {
        public event BytesPipeSentHandler? BytesSent;

        public BytesPipeSender(PipeStream pipe, bool compressed = true)
            : base(pipe, compressed) { }

        public int Send(byte[] bytes, TimeSpan? timeout = null)
            => Raise(SendAsync(bytes, Cancels.Token(timeout)).Result);

        public async Task<int> SendAsync(byte[] bytes, CancellationToken cancel)
        {
            byte[] deflated = Deflate(bytes, Compressed);
            byte[] lengthBytes = IntToBytes(deflated.Length);

            await Pipe.WriteAsync(lengthBytes, 0, lengthBytes.Length, cancel);
            await Pipe.WriteAsync(deflated, 0, deflated.Length, cancel);

            return deflated.Length;
        }

        private int Raise(int count)
        {
            if (BytesSent is not null)
                BytesSent.Invoke(this, new(count, Compressed));

            return count;
        }
    }
}
