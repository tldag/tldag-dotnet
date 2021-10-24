using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using TLDAG.DotNetLogger.IO;
using TLDAG.DotNetLogger.Model;
using TLDAG.DotNetLogger.Threading;
using static TLDAG.DotNetLogger.IO.Serialization;

namespace TLDAG.DotNetLogger
{
    public class ReceiverBuildReceivedEventArgs : EventArgs
    {
        public Log Log{ get; }

        public ReceiverBuildReceivedEventArgs(Log log) { Log = log; }
    }

    public delegate void ReceiverBuildReceivedHandler(Receiver receiver, ReceiverBuildReceivedEventArgs args);

    public class Receiver : IDisposable
    {
        private readonly AnonymousPipeServerStream pipe;
        private readonly BytesPipeReceiver receiver;

        private long count = 0;

        public string SenderDescriptor { get => Sender.GetDescriptor(pipe.GetClientHandleAsString()); }

        public event ReceiverBuildReceivedHandler? BuildReceived;

        public Receiver(ReceiverBuildReceivedHandler handler)
        {
            pipe = new(PipeDirection.In, HandleInheritability.Inheritable);
            receiver = new(pipe, BytesReceived);

            BuildReceived += handler;
        }

        ~Receiver() { Dispose(false); }
        public void Dispose() { Dispose(true); }
        private void Dispose(bool _) { receiver.Dispose(); pipe.Dispose(); }

        public long Wait(long expected, TimeSpan? timeout = null)
        {
            using Cancels cancels = new();
            Task<long> task = new(() => WaitAsync(expected).Result);

            task.Start(); task.Wait(cancels.Token(timeout));

            return task.Result;
        }

        private async Task<long> WaitAsync(long expected)
        {
            while (GetCount() < expected) await Task.Yield();
            return GetCount();
        }

        private void BytesReceived(BytesPipeReceiver _, BytesPipeReceivedEventArgs args)
        {
            if (BuildReceived is not null)
            {
                Log log = FromBytes(args.Bytes);

                log.Transferred = args.Received;

                BuildReceived.Invoke(this, new(log));
            }

            IncrementCount();
        }

        private long GetCount() => Interlocked.Read(ref count);
        private void IncrementCount() { Interlocked.Increment(ref count); }
    }
}
