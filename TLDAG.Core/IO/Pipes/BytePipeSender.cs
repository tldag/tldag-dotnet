using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.IO.Pipes
{
    public class BytePipeSentEventArgs : EventArgs
    {
        public int Count { get; }

        public BytePipeSentEventArgs(int count) { Count = count; }
    }

    public delegate void BytePipeSentHandler(BytePipeSender sender, BytePipeSentEventArgs args);

    public class BytePipeSender : BytePipeBase
    {
        public event BytePipeSentHandler? BytesSent;

        public BytePipeSender(PipeStream pipe) : base(pipe) { }

        public async Task<int> SendAsync(byte[] bytes, CancellationToken cancel)
        {
            byte[] lengthBytes = IntToBytes(bytes.Length);

            await Pipe.WriteAsync(lengthBytes, 0, lengthBytes.Length, cancel);
            await Pipe.WriteAsync(bytes, 0, bytes.Length, cancel);

            return lengthBytes.Length + bytes.Length;
        }

        public int Send(byte[] bytes, TimeSpan? timeout = null)
            => Raise(SendAsync(bytes, Cancellation.Token(timeout)).Result);

        private int Raise(int count)
        {
            if (BytesSent is not null)
                BytesSent.Invoke(this, new(count));

            return count;
        }
    }
}
