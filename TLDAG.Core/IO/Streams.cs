using System;
using System.IO;
using static TLDAG.Core.Algorithms.Conversion;

namespace TLDAG.Core.IO
{
    public abstract class StreamBase
    {
        protected readonly Stream stream;
        protected readonly byte[] smallBuffer = new byte[32];

        public StreamBase(Stream stream)
        {
            this.stream = stream;
        }

        internal void WriteCount(int count)
        {
            ToBytes(count, smallBuffer, 0);
            stream.Write(smallBuffer, 0, sizeof(int));
        }

        internal int ReadCount()
        {
            stream.Read(smallBuffer, 0, sizeof(int));
            return ToInt(smallBuffer, 0);
        }
    }

    public abstract class BasicTypeStream<T> : StreamBase
    {
        protected readonly int elementSize;

        public BasicTypeStream(Stream stream, int elementSize)
            : base(stream)
        {
            this.elementSize = elementSize;
        }

        public T[] Read()
            => Read(ReadCount());

        public T[] Read(int count)
        {
            if (count == 0) return Array.Empty<T>();

            T[] values = new T[count];

            Read(values, 0, count);

            return values;
        }

        public void Read(T[] values, int offset, int count)
        {
            if (count == 0) return;

            byte[] buffer = GetBuffer(count);
            stream.Read(buffer, 0, count * elementSize);

            Convert(buffer, values, offset, count);
        }

        public void Write(T[] values, bool writeCount = false)
            => Write(values, 0, values.Length, writeCount);

        public void Write(T[] values, int offset, int count, bool writeCount = false)
        {
            if (writeCount) WriteCount(count);

            if (count == 0) return;

            byte[] buffer = GetBuffer(count);

            Convert(values, offset, buffer, count);
            stream.Write(buffer, 0, count * elementSize);
        }

        protected virtual byte[] GetBuffer(int count)
        {
            int byteCount = count * elementSize;

            if (byteCount <= smallBuffer.Length) return smallBuffer;

            return new byte[byteCount];
        }

        protected abstract void Convert(byte[] src, T[] dest, int destOffset, int count);
        protected abstract void Convert(T[] src, int srcOffset, byte[] dest, int count);
    }

    public class ShortStream : BasicTypeStream<short>
    {
        public ShortStream(Stream stream) : base(stream, sizeof(short)) { }

        protected override void Convert(byte[] src, short[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(short[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class UShortStream : BasicTypeStream<ushort>
    {
        public UShortStream(Stream stream) : base(stream, sizeof(ushort)) { }

        protected override void Convert(byte[] src, ushort[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(ushort[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class IntStream : BasicTypeStream<int>
    {
        public IntStream(Stream stream) : base(stream, sizeof(int)) { }

        protected override void Convert(byte[] src, int[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(int[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class UIntStream : BasicTypeStream<uint>
    {
        public UIntStream(Stream stream) : base(stream, sizeof(uint)) { }

        protected override void Convert(byte[] src, uint[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(uint[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class LongStream : BasicTypeStream<long>
    {
        public LongStream(Stream stream) : base(stream, sizeof(long)) { }

        protected override void Convert(byte[] src, long[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(long[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class ULongStream : BasicTypeStream<ulong>
    {
        public ULongStream(Stream stream) : base(stream, sizeof(ulong)) { }

        protected override void Convert(byte[] src, ulong[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(ulong[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class FloatStream : BasicTypeStream<float>
    {
        public FloatStream(Stream stream) : base(stream, sizeof(float)) { }

        protected override void Convert(byte[] src, float[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(float[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class DoubleStream : BasicTypeStream<double>
    {
        public DoubleStream(Stream stream) : base(stream, sizeof(double)) { }

        protected override void Convert(byte[] src, double[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(double[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }

    public class CharStream : BasicTypeStream<char>
    {
        public CharStream(Stream stream) : base(stream, sizeof(char)) { }

        protected override void Convert(byte[] src, char[] dest, int destOffset, int count)
        {
            throw new NotImplementedException();
        }

        protected override void Convert(char[] src, int srcOffset, byte[] dest, int count)
        {
            throw new NotImplementedException();
        }
    }
}