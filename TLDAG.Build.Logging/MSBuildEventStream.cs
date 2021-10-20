using System;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventStream
    {
        private readonly PipeStream stream;
        private Task<string>? reader;

        public MSBuildEventStream(PipeStream stream)
        {
            this.stream = stream;
        }

        public void BeginRead()
        {
            reader = new(() => Read());
            reader.Start();
        }

        public string EndRead()
        {
            return reader?.Result ?? "";
        }

        private string Read()
        {
            byte[] bytes = new byte[sizeof(int)];

            stream.Read(bytes, 0, bytes.Length);

            int length = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[length];
            stream.Read(bytes, 0, length);

            return Encoding.UTF8.GetString(Inflate(bytes));
        }

        public void Write(string data)
        {
            byte[] bytes = Deflate(Encoding.UTF8.GetBytes(data));
            int length = bytes.Length;

            Console.WriteLine($"MSBuildEventStream transmits {length + sizeof(int)} bytes");

            stream.Write(BitConverter.GetBytes(length), 0, sizeof(int));
            stream.Write(bytes, 0, length);
        }

        private static byte[] Deflate(byte[] bytes)
        {
            MemoryStream memory = new();

            using (GZipStream stream = new(memory, CompressionLevel.Optimal))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }

            return memory.ToArray();  
        }

        private static byte[] Inflate(byte[] bytes)
        {
            MemoryStream input = new(bytes);
            MemoryStream output = new();

            using (GZipStream stream = new(input, CompressionMode.Decompress))
            {
                stream.CopyTo(output);
            }

            return output.ToArray();
        }
    }
}
