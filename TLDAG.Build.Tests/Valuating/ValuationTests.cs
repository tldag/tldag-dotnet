using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TLDAG.Build.Valuating;
using TLDAG.Core.IO;
using TLDAG.Test;
using static TLDAG.Build.Valuating.Valuation;

namespace TLDAG.Build.Tests.Valuating
{
    [TestClass]
    public class ValuationTests : TestsBase
    {
        //[TestMethod]
        //public void TestReport()
        //{
        //    DirectoryInfo directory = GetTestDirectory();
        //    FileInfo xmlOutput = directory.Combine("tldag-dotnet.value.xml");
        //    FileInfo mdOutput = directory.Combine("tldag-dotnet.value.md");
        //    ValuationReport report = ValuationReport.Create(SolutionFile.FullName, "CHF", 8);

        //    report.SaveToXml(xmlOutput);
        //    report.SaveToMarkdown(string.Empty, string.Empty, mdOutput);
        //}
    }
}
