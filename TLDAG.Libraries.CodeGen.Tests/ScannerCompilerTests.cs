using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Libraries.Core.CodeGen;

namespace TLDAG.Libraries.CodeGen.Tests
{
    [TestClass]
    public class ScannerCompilerTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            RexForest forest = RexForestBuilder.Create().AddTree(RexTrees.Figure_3_41()).Build();
            ScannerData data = ScannerCompiler.Compile(forest);

            Assert.AreEqual(3, data.Alphabet.Count);
        }
    }
}
