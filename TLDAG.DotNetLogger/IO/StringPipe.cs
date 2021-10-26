using System.IO.Pipes;
using System.Text;

namespace TLDAG.DotNetLogger.IO
{
    public abstract class StringPipe
    {
        protected abstract BytesPipe BytesPipe { get; }

        public PipeStream Pipe { get => BytesPipe.Pipe; }
        public Encoding Encoding { get; } = Encoding.UTF8;
    }
}
