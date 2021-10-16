using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Automation;
using TLDAG.Build.NuGet;
using TLDAG.Core.IO;

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
        public SwitchParameter Backup { get; set; } = new(false);

        protected override void Begin() { }
        protected override void End() { }

        protected override void Process()
        {
            DirectoryInfo root = Directories.Existing(Path);
            string? repository = string.IsNullOrEmpty(Repository) ? null : Repository;
            string? packages = string.IsNullOrEmpty(Packages) ? null : Packages;
            bool backup = Backup.ToBool();

            NuGets.Initialize(root, Name, repository, packages, backup);
        }
    }
}
