using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using TLDAG.Core.IO;
using TLDAG.Drawing;
using TLDAG.Test;
using static TLDAG.Build.Tests.Resources.Logos.LogoResources;

namespace TLDAG.Build.Tests.Drawing
{
    [TestClass]
    public class IconsTests : TestsBase
    {
        [TestMethod]
        public void MyTestMethod()
        {
            DirectoryInfo directory = GetTestDirectory(true);
            FileInfo icoFile = directory.Combine("TLDAG.ico");
            Bitmap[] bitmaps = { TLDAG_16, TLDAG_24 };
            
            Icons.Create(bitmaps, icoFile);
        }
    }
}
