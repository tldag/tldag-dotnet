using System;
using System.IO;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Primitives;

namespace TLDAG.Core.IO
{
    public abstract class PrimitiveStream
    {
        protected readonly Stream stream;
        protected readonly byte[] smallBuffer = new byte[32];

        public PrimitiveStream(Stream stream)
        {
            this.stream = stream;
        }

        protected void WriteCount(int count)
        {
            IntToBytes(count, smallBuffer, 0);
            stream.Write(smallBuffer, 0, sizeof(int));
        }

        protected int ReadCount()
        {
            stream.Read(smallBuffer, 0, sizeof(int));
            return ToInt(smallBuffer, 0);
        }
    }

    public abstract class PrimitiveStream<T> : PrimitiveStream
    {
        protected readonly int elementSize;

        public PrimitiveStream(Stream stream, int elementSize)
            : base(stream)
        {
            this.elementSize = elementSize;
        }

        public T[] Read() => Read(ReadCount());

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

    public class ShortStream : PrimitiveStream<short>
    {
        public ShortStream(Stream stream) : base(stream, sizeof(short)) { }

        protected override void Convert(byte[] src, short[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(short[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class UShortStream : PrimitiveStream<ushort>
    {
        public UShortStream(Stream stream) : base(stream, sizeof(ushort)) { }

        protected override void Convert(byte[] src, ushort[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(ushort[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class IntStream : PrimitiveStream<int>
    {
        public IntStream(Stream stream) : base(stream, sizeof(int)) { }

        protected override void Convert(byte[] src, int[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(int[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class UIntStream : PrimitiveStream<uint>
    {
        public UIntStream(Stream stream) : base(stream, sizeof(uint)) { }

        protected override void Convert(byte[] src, uint[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(uint[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class LongStream : PrimitiveStream<long>
    {
        public LongStream(Stream stream) : base(stream, sizeof(long)) { }

        protected override void Convert(byte[] src, long[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(long[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class ULongStream : PrimitiveStream<ulong>
    {
        public ULongStream(Stream stream) : base(stream, sizeof(ulong)) { }

        protected override void Convert(byte[] src, ulong[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(ulong[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class FloatStream : PrimitiveStream<float>
    {
        public FloatStream(Stream stream) : base(stream, sizeof(float)) { }

        protected override void Convert(byte[] src, float[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(float[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class DoubleStream : PrimitiveStream<double>
    {
        public DoubleStream(Stream stream) : base(stream, sizeof(double)) { }

        protected override void Convert(byte[] src, double[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(double[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }

    public class CharStream : PrimitiveStream<char>
    {
        public CharStream(Stream stream) : base(stream, sizeof(char)) { }

        protected override void Convert(byte[] src, char[] dest, int destOffset, int count)
        {
            throw NotYetImplemented();
        }

        protected override void Convert(char[] src, int srcOffset, byte[] dest, int count)
        {
            throw NotYetImplemented();
        }
    }
}
