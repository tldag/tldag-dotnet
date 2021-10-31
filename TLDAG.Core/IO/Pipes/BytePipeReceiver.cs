using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.IO.Pipes
{
    public class BytePipeReceivedEventArgs : EventArgs
    {
        public byte[] Bytes { get; }
        public int Received { get; }

        public BytePipeReceivedEventArgs(byte[] bytes, int received) { Bytes = bytes; Received = received; }
    }

    public delegate void BytePipeReceivedHandler(BytePipeReceiver receiver, BytePipeReceivedEventArgs args);

    public class BytePipeReceiver : BytePipeBase
    {
        public event BytePipeReceivedHandler? BytesReceived;

        public BytePipeReceiver(PipeStream pipe, BytePipeReceivedHandler handler, TimeSpan? timeout = null) : base(pipe)
        {
            BytesReceived += handler;

            Read(Cancellation.Token(timeout)).Start();
        }

        private Task Read(CancellationToken cancel)
            => new(async () => { while (!cancel.IsCancellationRequested) Raise(await ReadAsync(cancel)); });

        private async Task<BytePipeReceivedEventArgs> ReadAsync(CancellationToken cancel)
        {
            byte[] lengthBytes = IntToBytes(0);

            await Pipe.ReadAsync(lengthBytes, 0, lengthBytes.Length, cancel);

            int length = BytesToInt(lengthBytes);
            byte[] bytes = new byte[length];

            await Pipe.ReadAsync(bytes, 0, bytes.Length);

            return new(bytes, lengthBytes.Length + bytes.Length);
        }

        private void Raise(BytePipeReceivedEventArgs args)
            { if (BytesReceived is not null) BytesReceived.Invoke(this, args); }
    }
}
