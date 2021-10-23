﻿using System;
using System.IO.Pipes;

namespace TLDAG.DotNetLogger.IO
{
    public class StringPipeReceivedEventArgs : EventArgs
    {
        public string Text { get; }

        public StringPipeReceivedEventArgs(string text) { Text = text; }
    }

    public delegate void StringPipeReceivedHandler(StringPipeReceiver receiver, StringPipeReceivedEventArgs args);

    public class StringPipeReceiver : StringPipe, IDisposable
    {
        public event StringPipeReceivedHandler? StringReceived;

        private readonly BytesPipeReceiver _receiver;
        protected override BytesPipe BytesPipe => _receiver;

        public StringPipeReceiver(PipeStream pipe, StringPipeReceivedHandler handler, bool compressed = true,
            TimeSpan? timeout = null)
        {
            _receiver = new(pipe, BytesReceived, compressed, timeout);

            StringReceived += handler;
        }

        ~StringPipeReceiver() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { _receiver.Dispose(); }

        private void BytesReceived(BytesPipeReceiver receiver, BytesPipeReceivedEventArgs args)
        {
            if (StringReceived is not null)
                StringReceived.Invoke(this, new(Encoding.GetString(args.Bytes)));
        }
    }
}
