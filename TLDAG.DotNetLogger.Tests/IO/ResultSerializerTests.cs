using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TLDAG.Core.IO;
using TLDAG.DotNetLogger.Model;
using TLDAG.Test;
using static TLDAG.DotNetLogger.IO.ResultSerializer;

namespace TLDAG.DotNetLogger.Tests.IO
{
    [TestClass]
    public class ResultSerializerTests : TestsBase
    {
        private static readonly XmlWriterSettings TestSettings
            = new() { Indent = true, IndentChars = "  ", Encoding = Encoding.UTF8 };

        private Result CreateResult()
        {
            Result result = new();
            ProjectOld? project = result.GetProject(1, "foo.csproj");

            Assert.IsNotNull(project);

            return result;
        }

        [TestMethod]
        public void Test()
        {
            DirectoryInfo directory = GetTestDirectory(true);
            Result? result = CreateResult();
            string xml1, xml2;

            xml1 = Serialize(result, TestSettings);
            directory.Combine("1.xml").WriteAllText(xml1);

            result = Deserialize(xml1);
            Assert.IsNotNull(result);
            xml2 = Serialize(result, TestSettings);
            directory.Combine("2.xml").WriteAllText(xml2);

            Assert.AreEqual(xml1, xml2);
        }
    }
}
