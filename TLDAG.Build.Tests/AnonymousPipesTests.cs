using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO.Pipes;

namespace TLDAG.Build.Tests
{
    [TestClass]
    public class AnonymousPipesTests
    {
        [TestMethod]
        public void Test()
        {
            using AnonymousPipeServerStream server = new(PipeDirection.In);
            string handle = server.GetClientHandleAsString();
            using AnonymousPipeClientStream client = new(PipeDirection.Out, handle);

            byte expected = 5;

            client.WriteByte(expected);

            byte actual = (byte)server.ReadByte();

            Assert.AreEqual(expected, actual);
        }
    }
}
