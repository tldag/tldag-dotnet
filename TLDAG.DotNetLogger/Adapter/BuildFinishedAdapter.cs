using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildFinishedAdapter : BuildAdapter<BuildFinishedEventArgs>
    {
        public bool Success { get => Args.Succeeded; }

        public BuildFinishedAdapter(BuildFinishedEventArgs args) : base(args) { }
    }
}
