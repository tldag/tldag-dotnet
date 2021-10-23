using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace TLDAG.DotNetLogger.Algorithm
{
    public static class Deflater
    {
        public static byte[] Deflate(byte[] source, bool compress = true, bool fast = false)
        {
            if (!compress) return source;

            using MemoryStream memory = new();

            using (GZipStream stream = CreateGZipStream(memory, fast))
            {
                stream.Write(source, 0, source.Length);
                stream.Flush();
            }

            return memory.ToArray();
        }

        private static GZipStream CreateGZipStream(Stream source, bool fast)
        {
            CompressionLevel level = fast ? CompressionLevel.Fastest : CompressionLevel.Optimal;

            return new(source, level);
        }
    }

    public static class Inflater
    {
        public static byte[] Inflate(byte[] source, bool compressed = true)
        {
            if (!compressed) return source;

            using MemoryStream input = new(source);
            using MemoryStream output = new();

            using (GZipStream stream = new(input, CompressionMode.Decompress))
            {
                stream.CopyTo(output);
            }

            return output.ToArray();
        }
    }
}
