using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildErrorAdapter : BuildAdapter<BuildErrorEventArgs>
    {
        public override string? ProjectFile { get => Args.ProjectFile; }

        public BuildErrorAdapter(BuildErrorEventArgs args) : base(args) { }
    }
}
