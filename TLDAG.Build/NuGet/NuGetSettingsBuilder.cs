using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.IO;
using static TLDAG.Build.NuGet.NuGets;

namespace TLDAG.Build.NuGet
{
    public class NuGetSettingsBuilder
    {
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
            : this(root, DefaultSettingsFileName, backup) { }

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

            ClearSection(settings, SourcesSection);
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
                settings.AddOrUpdate(ConfigSection, new AddItem(GlobalPackagesFolderKey, globalPackages));

            if (repository != null)
                settings.AddOrUpdate(ConfigSection, new AddItem(RepositoryPathKey, repository));
        }

        private void InitSources(Settings settings)
        {
            if (clearSources)
                settings.AddOrUpdate(SourcesSection, new ClearItem());

            foreach (SourceItem source in sources)
                settings.AddOrUpdate(SourcesSection, source);
        }

        private void InitFallback(Settings settings)
        {
            if (clearFallback)
                settings.AddOrUpdate(FallbackSection, new ClearItem());
        }

        private void InitDisabled(Settings settings)
        {
            if (clearDisabled)
                settings.AddOrUpdate(DisabledSection, new ClearItem());
        }

        private static void ClearSection(Settings settings, string name)
        {
            SettingSection section = settings.GetSection(name);
            SettingItem[] items = section.Items.ToArray();

            foreach (SettingItem item in items) settings.Remove(name, item);
        }
    }
}
