using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Build.NuGet
{
    public class NuGetSettingsBuilder
    {
        public static readonly Uri NuGetApiUri = new("https://api.nuget.org/v3/index.json");

        private readonly DirectoryInfo root;
        private readonly string name;
        private readonly bool backup;

        private string? globalPackages = null;
        private string? repository = null;

        private bool clearSources = false;
        private bool clearFallback = false;
        private bool clearDisabled = false;

        private readonly List<SourceItem> sources = new();

        public NuGetSettingsBuilder(DirectoryInfo root, string name, bool backup)
        {
            this.root = root;
            this.name = name;
            this.backup = backup;
        }

        public NuGetSettingsBuilder(DirectoryInfo root, bool backup)
            : this(root, Settings.DefaultSettingsFileName, backup) { }

        public static NuGetSettingsBuilder Create(DirectoryInfo root, string name, bool backup)
            => new(root, name, backup);

        public static NuGetSettingsBuilder Create(DirectoryInfo root, bool backup)
            => new(root, backup);

        public NuGetSettingsBuilder GlobalPackages(string directory)
            { globalPackages = directory; return this; }

        public NuGetSettingsBuilder Repository(string directory)
            { repository = directory; return this; }

        public NuGetSettingsBuilder ClearSources() { clearSources = true; return this; }
        public NuGetSettingsBuilder ClearFallback() { clearFallback = true; return this; }
        public NuGetSettingsBuilder ClearDisabled() { clearDisabled = true; return this; }

        public NuGetSettingsBuilder AddSource(string key, Uri uri, string protocolVersion)
            { sources.Add(new(key, uri.AbsoluteUri, protocolVersion)); return this; }

        public NuGetSettingsBuilder AddSource(string key, DirectoryInfo directory)
            => AddSource(key, directory.ToUri(), "");

        public NuGetSettingsBuilder AddNuGetSource(string key) => AddSource(key, NuGetApiUri, "3");

        public Settings Build()
        {
            FileInfo file = root.Combine(name);

            if (file.Exists && backup) file.Backup();

            Settings settings = new(root.FullName, name);

            ClearSection(settings, "packageSources");
            InitConfig(settings);
            InitSources(settings);
            InitFallback(settings);
            InitDisabled(settings);

            settings.SaveToDisk();

            return settings;
        }

        private void InitConfig(Settings settings)
        {
            if (globalPackages != null)
                settings.AddOrUpdate("config", new AddItem("globalPackagesFolder", globalPackages));

            if (repository != null)
                settings.AddOrUpdate("config", new AddItem("repositoryPath", repository));
        }

        private void InitSources(Settings settings)
        {
            if (clearSources)
                settings.AddOrUpdate("packageSources", new ClearItem());

            foreach (SourceItem source in sources)
                settings.AddOrUpdate("packageSources", source);
        }

        private void InitFallback(Settings settings)
        {
            if (clearFallback)
                settings.AddOrUpdate("fallbackPackageFolders", new ClearItem());
        }

        private void InitDisabled(Settings settings)
        {
            if (clearDisabled)
                settings.AddOrUpdate("disabledPackageSources", new ClearItem());
        }

        private static void ClearSection(Settings settings, string name)
        {
            SettingSection section = settings.GetSection(name);
            SettingItem[] items = section.Items.ToArray();

            foreach (SettingItem item in items) settings.Remove(name, item);
        }
    }
}
