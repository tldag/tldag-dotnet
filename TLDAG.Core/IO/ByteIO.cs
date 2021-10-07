using System.IO;
using System.IO.Compression;

namespace TLDAG.Core.IO
{
    public static class ByteIO
    {
        public static Stream ToStream(byte[] bytes, bool compressed)
        {
            Stream stream = new MemoryStream(bytes);

            if (compressed) stream = new GZipStream(stream, CompressionMode.Decompress);

            return stream;
        }
    }
}
