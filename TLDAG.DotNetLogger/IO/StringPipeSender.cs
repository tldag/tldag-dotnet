using System;
using System.IO.Pipes;
using System.Text;

namespace TLDAG.DotNetLogger.IO
{
    public class StringPipeSentEventArgs : BytesPipeSentEventArgs
    {
        public int Length { get; }

        public StringPipeSentEventArgs(int count, bool compressed, int length)
            : base(count, compressed) { Length = length; }
    }

    public delegate void StringPipeSentHandler(StringPipeSender sender, StringPipeSentEventArgs args);

    public class StringPipeSender : StringPipe, IDisposable
    {
        public event StringPipeSentHandler? StringSent;

        private readonly BytesPipeSender sender;
        protected override BytesPipe BytesPipe => sender;

        public StringPipeSender(PipeStream pipe, bool compressed = true)
        {
            sender = new(pipe, compressed);
        }

        ~StringPipeSender() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { sender.Dispose(); }

        public int Send(string text, TimeSpan? timeout = null)
            => Raise(sender.Send(Encoding.GetBytes(text), timeout), text.Length);

        private int Raise(int count, int length)
        {
            if (StringSent is not null)
                StringSent.Invoke(this, new(count, Compressed, length));

            return count;
        }
    }
}
