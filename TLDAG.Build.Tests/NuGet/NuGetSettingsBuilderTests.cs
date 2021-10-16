using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Configuration;
using System.Diagnostics;
using System.IO;
using TLDAG.Build.NuGet;
using TLDAG.Core.IO;
using TLDAG.Test;

namespace TLDAG.Build.Tests.NuGet
{
    [TestClass]
    public class NuGetSettingsBuilderTests : TestsBase
    {
        [TestMethod]
        public void TestEmpty()
        {
            DirectoryInfo root = GetTestDirectory(true);
            DirectoryInfo packages = root.CombineDirectory("packages");

            Settings settings = NuGetSettingsBuilder.Create(root, false)
                .ClearSources().ClearFallback().ClearDisabled()
                .GlobalPackages("repository").Repository("repository")
                .AddNuGetSource("NuGet").AddSource("Packages", packages)
                .Build();

            Debug.WriteLine(settings);
        }
    }
}
