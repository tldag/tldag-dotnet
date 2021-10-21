using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using TLDAG.Core.IO;
using TLDAG.Test;
using static TLDAG.Build.Logging.MSBuildEventModel;

namespace TLDAG.Build.Logging.Tests
{
    [TestClass]
    public class MSBuildEventModelTests : TestsBase
    {
        private static XmlWriterSettings Settings { get; } = new()
        {
            Indent = true,
            IndentChars = "  "
        };

        [TestMethod]
        public void Test()
        {
            DirectoryInfo directory = GetTestDirectory(true);
            FileInfo file = directory.Combine("test.xml");
            BuildResult result = new();

            result.AddProjectData(new(1, "foo.csproj"));
            result.AddProjectData(new(2, "bar.csproj"));

            file.WriteAllText(result.Serialize(Settings));
        }
    }
}
