using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TLDAG.Build.Tests
{
    [TestClass]
    public class NuGetTests
    {
        private readonly ILogger logger = NullLogger.Instance;
        private readonly CancellationToken cancel = CancellationToken.None;
        private readonly SourceCacheContext cache = new SourceCacheContext();

        //[TestMethod]
        public void ListPackageVersions()
        {
            SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            FindPackageByIdResource resource = repository.GetResource<FindPackageByIdResource>();
            IEnumerable<NuGetVersion> versions = resource.GetAllVersionsAsync("Newtonsoft.Json", cache, logger, cancel).Result;

            foreach (NuGetVersion version in versions)
                Debug.WriteLine(version);
        }

        [TestMethod]
        public void LoadDefaultSettings()
        {
            string root = Environment.CurrentDirectory;
            ISettings settings = Settings.LoadDefaultSettings(root);
            SettingSection? packageSources = settings.GetSection("packageSources");

            if (packageSources is null) throw new NotSupportedException();

            foreach (SettingItem item in packageSources.Items)
            {
                Debug.WriteLine(item);
            }
        }

        [TestMethod]
        public void PackageSources()
        {
            string root = Environment.CurrentDirectory;
            ISettings settings = Settings.LoadDefaultSettings(root);
            PackageSourceProvider provider = new(settings);
            IEnumerable<PackageSource> sources = provider.LoadPackageSources();

            foreach (PackageSource source in sources)
            {
                Debug.WriteLine(source);
            }
        }

        [TestMethod]
        public void LocalTldagSdkVersions()
        {
            string root = Environment.CurrentDirectory;
            ISettings settings = Settings.LoadDefaultSettings(root);
            PackageSourceProvider provider = new(settings);
            PackageSource packageSource = provider.GetPackageSourceByName("Packages");
            SourceRepository repository = Repository.Factory.GetCoreV3(packageSource.Source);
            FindPackageByIdResource resource = repository.GetResource<FindPackageByIdResource>();
            IEnumerable<NuGetVersion> versions = resource.GetAllVersionsAsync("TLDAG.Sdk", cache, logger, cancel).Result;

            foreach (NuGetVersion version in versions)
                Debug.WriteLine(version);
        }
    }
}
