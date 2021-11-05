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
        [TestMethod]
        public void TestGetInputType()
        {
            FileInfo projectFile = SolutionDirectory.Combine("TLDAG.Build.Tests", "TLDAG.Build.Tests.csproj");

            Assert.AreEqual(InputType.Solution, GetInputType(SolutionFile.FullName));
            Assert.AreEqual(InputType.Project, GetInputType(projectFile.FullName));
            Assert.AreEqual(InputType.Directory, GetInputType(SolutionDirectory.FullName));
        }

        [TestMethod]
        public void TestGetProjects()
        {
            FileInfo projectFile = SolutionDirectory.Combine("TLDAG.Build.Tests", "TLDAG.Build.Tests.csproj");
            ProjectInfo[] projects;

            projects = GetProjects(SolutionFile.FullName).ToArray();
            Assert.IsTrue(projects.Length > 0);

            projects = GetProjects(projectFile.FullName).ToArray();
            Assert.AreEqual(1, projects.Length);

            projects = GetProjects(SolutionDirectory.FullName).ToArray();
            Assert.IsTrue(projects.Length > 0);
        }

        [TestMethod]
        public void TestGetSources()
        {
            FileInfo projectFile = SolutionDirectory.Combine("TLDAG.Build.Tests", "TLDAG.Build.Tests.csproj");
            SourceInfo[] sources = GetSources(projectFile.FullName).ToArray();

            Assert.IsTrue(sources.Length > 0);
        }

        [TestMethod]
        public void TestReport()
        {
            DirectoryInfo directory = GetTestDirectory(true);
            FileInfo xmlOutput = directory.Combine("tldag-dotnet.value.xml");
            FileInfo mdOutput = directory.Combine("tldag-dotnet.value.md");
            ValuationReport report = ValuationReport.Create(SolutionFile.FullName, "CHF", 8);

            report.SaveToXml(xmlOutput);
            report.SaveToMarkdown(string.Empty, string.Empty, mdOutput);
        }
    }
}
