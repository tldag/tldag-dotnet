using NuGet.Configuration;
using System;
using System.IO;
using TLDAG.Core;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Build.NuGet
{
    public static class NuGets
    {
        public static readonly string DefaultSettingsFileName = Settings.DefaultSettingsFileName;
        public static readonly Uri NuGetApiUri = new("https://api.nuget.org/v3/index.json");

        public const string ConfigSection = "config";
        public const string SourcesSection = "packageSources";
        public const string FallbackSection = "fallbackPackageFolders";
        public const string DisabledSection = "disabledPackageSources";

        public const string GlobalPackagesFolderKey = "globalPackagesFolder";
        public const string RepositoryPathKey = "repositoryPath";

        public static ISettings GetSettings(DirectoryInfo directory)
            => Settings.LoadDefaultSettings(directory.FullName);

        public static ISettings CurrentSettings => GetSettings(Env.CurrentDirectory);

        public static ISettings Initialize(DirectoryInfo root, string? name, string? repository,
            string? packages, bool backup)
        {
            throw NotYetImplemented();
        }

        public static SourceItem[] Sources(ISettings? settings = null)
        {
            throw NotYetImplemented();
        }
    }
}
