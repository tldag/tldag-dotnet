using NuGet.Configuration;
using System.IO;
using TLDAG.Core;

namespace TLDAG.Build.NuGet
{
    public static class NuGets
    {
        public static ISettings GetSettings(DirectoryInfo directory)
            => global::NuGet.Configuration.Settings.LoadDefaultSettings(directory.FullName);

        public static ISettings Settings => GetSettings(Env.CurrentDirectory);
    }
}
