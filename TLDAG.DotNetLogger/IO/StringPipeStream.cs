using System;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using static TLDAG.DotNetLogger.Conversion.Primitives;

namespace TLDAG.DotNetLogger.IO
{
    public class StringPipeStream
    {
        private readonly PipeStream stream;
        private readonly bool compressed;
        private Task<string>? reader = null;

        public StringPipeStream(PipeStream stream, bool compressed = true)
        {
            this.stream = stream;
            this.compressed = compressed;
        }

        public void BeginRead()
        {
            if (reader is not null)
                throw new NotSupportedException();

            reader = new(() => Read());
            reader.Start();
        }

        public string EndRead()
        {
            if (reader is null)
                throw new NotSupportedException();

            string result = reader.Result;

            reader = null;

            return result;
        }

        public void Write(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            if (compressed)
                bytes = Deflate(bytes);

            int length = bytes.Length;
            byte[] lengthBytes = IntToBytes(length);

            stream.Write(lengthBytes, 0, lengthBytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        private string Read()
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            stream.Read(lengthBytes, 0, lengthBytes.Length);

            int length = BytesToInt(lengthBytes);
            byte[] bytes = new byte[length];

            stream.Read(bytes, 0, bytes.Length);

            if (compressed)
                bytes = Inflate(bytes);

            return Encoding.UTF8.GetString(bytes);
        }

        private static byte[] Inflate(byte[] bytes)
        {
            using MemoryStream input = new(bytes);
            using MemoryStream output = new();

            using (GZipStream stream = new(input, CompressionMode.Decompress))
            {
                stream.CopyTo(output);
            }

            return output.ToArray();
        }

        private static byte[] Deflate(byte[] bytes)
        {
            using MemoryStream memory = new();

            using (GZipStream stream = new(memory, CompressionLevel.Optimal))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }

            return memory.ToArray();
        }
    }
}
