using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildWarningAdapter : BuildAdapter<BuildWarningEventArgs>
    {
        public override string ProjectFile { get => Args.ProjectFile ?? base.ProjectFile; }

        public BuildWarningAdapter(BuildWarningEventArgs args) : base(args) { }
    }
}
