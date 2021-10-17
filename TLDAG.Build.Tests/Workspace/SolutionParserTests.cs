using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Build.Workspace.Internal;
using TLDAG.Test;

namespace TLDAG.Build.Tests.Workspace
{
    [TestClass]
    public class SolutionParserTests : TestsBase
    {
        [TestMethod]
        public void Test()
        {
            SolutionParser.Parse(SolutionFile);
        }
    }
}
