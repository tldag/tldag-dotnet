using Microsoft.Build.Framework;

namespace TLDAG.DotNetLogger.Adapter
{
    public class BuildStartedAdapter : BuildAdapter
    {
        public BuildStartedAdapter(BuildStartedEventArgs args) : base(args) { }
    }
}
