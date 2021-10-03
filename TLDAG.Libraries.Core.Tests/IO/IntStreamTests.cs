using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TLDAG.Libraries.Core.IO;

namespace TLDAG.Libraries.Core.Tests.IO
{
    [TestClass]
    public class IntStreamTests
    {
        [TestMethod]
        public void Conversion()
        {
            int expected = 689;
            byte[] bytes = new byte[4];

            bytes[0] = (byte)(expected >> 24);
            bytes[1] = (byte)(expected >> 16);
            bytes[2] = (byte)(expected >> 8);
            bytes[3] = (byte)expected;

            int actual = 0;

            actual |= bytes[3];
            actual |= ((int)bytes[2]) << 8;
            actual |= ((int)bytes[1]) << 16;
            actual |= ((int)bytes[0]) << 24;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadWrite()
        {
            int[] expected = { 36, 8645, -356, 0 };

            MemoryStream output = new();
            IntStream writer = new(output);

            writer.Write(expected);

            Assert.AreEqual(16, output.Length);
            Assert.AreEqual(16, output.Position);

            MemoryStream input = new(output.ToArray());
            IntStream reader = new(input);
            int[] actual = new int[4];

            reader.Read(actual);

            for (int i = 0; i < 4; ++i)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
