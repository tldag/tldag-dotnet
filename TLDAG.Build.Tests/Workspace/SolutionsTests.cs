using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Build.Workspace;
using TLDAG.Test;

namespace TLDAG.Build.Tests.Workspace
{
    [TestClass]
    public class SolutionsTests : TestsBase
    {
        [TestMethod]
        public void Test()
        {
            Solutions.GetWorkspace(SolutionFile);
        }
    }
}
