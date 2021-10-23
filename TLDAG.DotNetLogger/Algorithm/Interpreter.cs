using Microsoft.Build.Framework;
using TLDAG.DotNetLogger.Adapter;
using TLDAG.DotNetLogger.Model;

namespace TLDAG.DotNetLogger.Algorithm
{
    public static class Interpreter
    {
        public static void Transfer(BuildStartedEventArgs args, Builds builds)
            => Transfer(new BuildStartedAdapter(args), builds);

        public static Build Transfer(BuildFinishedEventArgs args, Builds builds)
            => Transfer(new BuildFinishedAdapter(args), builds);

        public static void Transfer(BuildStartedAdapter args, Builds builds)
        {
            Build build = builds.StartBuild();

            Transfer(args, build);
        }

        public static Build Transfer(BuildFinishedAdapter args, Builds builds)
        {
            Build build = builds.EndBuild();

            Transfer(args, build);

            return build;
        }

        private static void Transfer(BuildAdapter args, Build build)
        {
        }
    }
}
