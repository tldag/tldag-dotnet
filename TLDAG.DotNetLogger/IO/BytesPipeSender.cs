using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using static TLDAG.DotNetLogger.Conversion.Primitives;

namespace TLDAG.DotNetLogger.IO
{
    public class BytesPipeSentEventArgs : EventArgs
    {
        public int Count { get; }

        public BytesPipeSentEventArgs(int count) { Count = count; }
    }

    public delegate void BytesPipeSentHandler(BytesPipeSender sender, BytesPipeSentEventArgs args);

    public class BytesPipeSender : BytesPipe
    {
        public event BytesPipeSentHandler? BytesSent;

        public BytesPipeSender(PipeStream pipe)
            : base(pipe) { }

        public int Send(byte[] bytes, TimeSpan? timeout = null)
            => Raise(SendAsync(bytes, Cancels.Token(timeout)).Result);

        public async Task<int> SendAsync(byte[] bytes, CancellationToken cancel)
        {
            byte[] lengthBytes = IntToBytes(bytes.Length);

            await Pipe.WriteAsync(lengthBytes, 0, lengthBytes.Length, cancel);
            await Pipe.WriteAsync(bytes, 0, bytes.Length, cancel);

            return lengthBytes.Length + bytes.Length;
        }

        private int Raise(int count)
        {
            if (BytesSent is not null)
                BytesSent.Invoke(this, new(count));

            return count;
        }
    }
}
