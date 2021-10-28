using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.IO.DnlSerialization;

namespace TLDAG.DotNetLogger.Tests.IO
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Test()
        {
            DnlLog log = new();

            ToBytes(log);
        }
    }
}
