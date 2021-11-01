using NuGet.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using TLDAG.Automation;
using TLDAG.Build.NuGet;
using TLDAG.Core.IO;
using TLDAG.Core.Net;
using static TLDAG.Core.IO.Directories;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsData.Initialize, "NuGet")]
    public class InitializeNuGet : Command
    {
        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 0)]
        public string Path { get; set; } = "";

        [Parameter(Mandatory = false, ValueFromPipeline = false)]
        public string Name { get; set; } = NuGets.DefaultSettingsFileName;

        [Parameter(Mandatory = false, ValueFromPipeline = false)]
        public string Repository { get; set; } = "";

        [Parameter(Mandatory = false, ValueFromPipeline = false)]
        public string Packages { get; set; } = "";

        [Parameter()]
        public SwitchParameter Backup { get; set; } = false;

        [Parameter()]
        public SwitchParameter Defaults { get; set; } = false;

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            if (Defaults)
            {
                Name = NuGets.DefaultSettingsFileName;
                Repository = "repository";
                Packages = "packages";
            }

            DirectoryInfo root = ExistingDirectory(Path);
            string? repository = string.IsNullOrEmpty(Repository) ? null : Repository;
            string? packages = string.IsNullOrEmpty(Packages) ? null : Packages;
            bool backup = Backup.ToBool();
            ISettings settings = NuGets.Initialize(root, Name, repository, packages, backup);

            NuGets.Sources(settings)
                .Select(s => new Uri(s.Value))
                .Where(u => u.IsFile)
                .Select(u => u.ToDirectory())
                .ToList().ForEach(d => d.Create());

            if (repository != null)
                root.CombineDirectory(repository).Create();
        }
    }
}
