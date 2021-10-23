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

        public BytesPipeReceivedEventArgs(byte[] bytes) { Bytes = bytes; }
    }

    public delegate void BytesPipeReceivedHandler(BytesPipeReceiver receiver, BytesPipeReceivedEventArgs args);

    public class BytesPipeReceiver : BytesPipe, IDisposable
    {
        public event BytesPipeReceivedHandler? BytesReceived;

        public BytesPipeReceiver(PipeStream pipe, BytesPipeReceivedHandler handler, bool compressed = true,
            TimeSpan? timeout = null) : base(pipe, compressed, timeout)
        {
            BytesReceived += handler;

            Read(Cancel.Token).Start();
        }

        ~BytesPipeReceiver() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { Cancel.Cancel(false); }

        private Task Read(CancellationToken cancel)
            => new(async () => { while (!cancel.IsCancellationRequested) Raise(await ReadAsync(cancel)); });

        private async Task<byte[]> ReadAsync(CancellationToken cancel)
        {
            byte[] lengthBytes = IntToBytes(0);

            await Pipe.ReadAsync(lengthBytes, 0, lengthBytes.Length, cancel);

            int length = BytesToInt(lengthBytes);
            byte[] bytes = new byte[length];

            await Pipe.ReadAsync(bytes, 0, bytes.Length);

            return Inflate(bytes, Compressed);
        }

        private void Raise(byte[] bytes) { if (BytesReceived is not null) BytesReceived.Invoke(this, new(bytes)); }
    }
}
