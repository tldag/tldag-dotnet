using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using static TLDAG.DotNetLogger.Algorithm.Inflater;
using static TLDAG.DotNetLogger.Conversion.Primitives;

namespace TLDAG.DotNetLogger.IO
{
    public class BytesPipeReceivedEventArgs : EventArgs
    {
        public byte[] Bytes { get; }
        public int Received { get; }

        public BytesPipeReceivedEventArgs(byte[] bytes, int received) { Bytes = bytes; Received = received; }
    }

    public delegate void BytesPipeReceivedHandler(BytesPipeReceiver receiver, BytesPipeReceivedEventArgs args);

    public class BytesPipeReceiver : BytesPipe
    {
        public event BytesPipeReceivedHandler? BytesReceived;

        public BytesPipeReceiver(PipeStream pipe, BytesPipeReceivedHandler handler, bool compressed = true,
            TimeSpan? timeout = null) : base(pipe, compressed)
        {
            BytesReceived += handler;

            Read(Cancels.Token(timeout)).Start();
        }

        private Task Read(CancellationToken cancel)
            => new(async () => { while (!cancel.IsCancellationRequested) Raise(await ReadAsync(cancel)); });

        private async Task<BytesPipeReceivedEventArgs> ReadAsync(CancellationToken cancel)
        {
            byte[] lengthBytes = IntToBytes(0);

            await Pipe.ReadAsync(lengthBytes, 0, lengthBytes.Length, cancel);

            int length = BytesToInt(lengthBytes);
            byte[] deflated = new byte[length];

            await Pipe.ReadAsync(deflated, 0, deflated.Length);

            byte[] inflated = Inflate(deflated, Compressed);

            return new(inflated, lengthBytes.Length + deflated.Length);
        }

        private void Raise(BytesPipeReceivedEventArgs args)
            { if (BytesReceived is not null) BytesReceived.Invoke(this, args); }
    }
}
