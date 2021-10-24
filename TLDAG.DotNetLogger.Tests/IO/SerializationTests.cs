using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.IO.Serialization;

namespace TLDAG.DotNetLogger.Tests.IO
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Test()
        {
            Log log = new();

            ToBytes(log);
        }
    }
}
