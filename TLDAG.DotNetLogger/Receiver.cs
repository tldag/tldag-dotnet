using System;
using System.IO;
using System.IO.Pipes;
using TLDAG.DotNetLogger.IO;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.IO.Serialization;

namespace TLDAG.DotNetLogger
{
    public class ReceiverBuildReceivedEventArgs : EventArgs
    {
        public Build Build { get; }

        public ReceiverBuildReceivedEventArgs(Build build) { Build = build; }
    }

    public delegate void ReceiverBuildReceivedHandler(Receiver receiver, ReceiverBuildReceivedEventArgs args);

    public class Receiver : IDisposable
    {
        private readonly AnonymousPipeServerStream _pipe;
        private readonly BytesPipeReceiver _receiver;

        public string SenderDescriptor { get => Sender.GetDescriptor(_pipe.GetClientHandleAsString()); }

        public event ReceiverBuildReceivedHandler? BuildReceived;

        public Receiver(ReceiverBuildReceivedHandler handler)
        {
            _pipe = new(PipeDirection.In, HandleInheritability.Inheritable);
            _receiver = new(_pipe, BytesReceived);

            BuildReceived += handler;
        }

        ~Receiver() { Dispose(false); }
        public void Dispose() { Dispose(true); }
        private void Dispose(bool _) { _receiver.Dispose(); _pipe.Dispose(); }

        private void BytesReceived(BytesPipeReceiver _, BytesPipeReceivedEventArgs args)
        {
            if (BuildReceived is not null)
            {
                Build build = FromBytes(args.Bytes);

                BuildReceived.Invoke(this, new(build));
            }
        }
    }
}
