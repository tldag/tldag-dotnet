using NuGet.Configuration;
using System;
using System.IO;
using System.Linq;
using TLDAG.Core;
using TLDAG.Core.IO;

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

        public const string NuGetSourceName = "NuGet";
        public const string PackagesSourceName = "Packages";

        public static ISettings GetSettings(DirectoryInfo directory)
            => Settings.LoadDefaultSettings(directory.FullName);

        public static ISettings CurrentSettings => GetSettings(Env.WorkingDirectory);

        public static ISettings Initialize(DirectoryInfo root, string? name, string? repository,
            string? packages, bool backup)
        {
            name ??= DefaultSettingsFileName;

            NuGetSettingsBuilder builder = new(root, name, backup);

            builder.ClearSources().ClearFallback().ClearDisabled()
                .AddNuGetSource(NuGetSourceName);

            if (repository != null)
                builder.GlobalPackages(repository).Repository(repository);

            if (packages != null)
                builder.AddSource(PackagesSourceName, root.CombineDirectory(packages));

            return builder.Build();
        }

        public static SourceItem[] Sources(ISettings? settings = null)
        {
            settings ??= CurrentSettings;

            return settings.GetSection(SourcesSection).Items
                .Where(item => item is SourceItem)
                .Cast<SourceItem>().ToArray();
        }
    }
}
