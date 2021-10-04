using System.Collections.Generic;
using System.IO;

namespace TLDAG.Libraries.Core.IO
{
    public class IntStreamOld
    {
        private readonly Stream stream;
        private readonly byte[] bytes = new byte[4];

        public IntStreamOld(Stream stream)
        {
            this.stream = stream;
        }

        public void Write(int value)
        {
            bytes[0] = (byte)(value >> 24);
            bytes[1] = (byte)(value >> 16);
            bytes[2] = (byte)(value >> 8);
            bytes[3] = (byte)value;

            stream.Write(bytes, 0, 4);
        }

        public void Write(IEnumerable<int> values)
        {
            foreach (int value in values)
            {
                Write(value);
            }
        }

        public int Read()
        {
            stream.Read(bytes, 0, 4);

            int value = 0;

            value |= bytes[3];
            value |= ((int)bytes[2]) << 8;
            value |= ((int)bytes[1]) << 16;
            value |= ((int)bytes[0]) << 24;

            return value;
        }

        public void Read(int[] values)
        {
            for (int i = 0, n = values.Length; i < n; ++i)
            {
                values[i] = Read();
            }
        }
    }
}
