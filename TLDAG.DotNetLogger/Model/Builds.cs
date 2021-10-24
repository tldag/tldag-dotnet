using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public class Builds
    {
        private readonly Stack<Build> builds = new();

        public Build StartBuild() { builds.Push(new()); return builds.Peek(); }
        public Build EndBuild() => builds.Pop();

        public Build GetBuild() => builds.Peek();
    }
}
