using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildFinishedAdapter : BuildAdapter<BuildFinishedEventArgs>
    {
        public bool Success { get => Args.Succeeded; }

        public BuildFinishedAdapter(BuildFinishedEventArgs args) : base(args) { }
        public BuildFinishedAdapter() : this(new(string.Empty, string.Empty, false)) { }

        public static implicit operator BuildFinishedAdapter(BuildFinishedEventArgs args) => new(args);
    }
}
