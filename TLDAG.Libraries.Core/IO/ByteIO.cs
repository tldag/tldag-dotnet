using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Libraries.Core.IO
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
