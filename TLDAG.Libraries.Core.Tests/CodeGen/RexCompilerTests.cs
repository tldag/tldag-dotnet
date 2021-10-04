using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Libraries.Core.CodeGen;

namespace TLDAG.Libraries.Core.Tests.CodeGen
{
    [TestClass]
    public class RexCompilerTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            RexForest forest = RexForestBuilder.Create().AddTree(RexTrees.Figure_3_41()).Build();
            ScannerData data = RexCompiler.Compile(forest);

            Assert.AreEqual(3, data.Alphabet.Count);
        }
    }
}
