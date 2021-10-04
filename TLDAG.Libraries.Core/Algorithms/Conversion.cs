namespace TLDAG.Libraries.Core.Algorithms
{
    public static class Conversion
    {
        public static void ToBytes(int value, byte[] dest, int destOffset)
        {
            dest[destOffset++] = (byte)(value >> 24);
            dest[destOffset++] = (byte)(value >> 16);
            dest[destOffset++] = (byte)(value >> 8);
            dest[destOffset] = (byte)value;
        }

        public static int ToInt(byte[] src, int srcOffset)
        {
            int value = 0;

            srcOffset += 3;

            value |= src[srcOffset--];
            value |= src[srcOffset--] << 8;
            value |= src[srcOffset--] << 16;
            value |= src[srcOffset] << 24;

            return value;
        }
    }
}
