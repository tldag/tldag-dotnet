using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public class Builds
    {
        private readonly Stack<Build> _builds = new();

        public Build StartBuild() { _builds.Push(new()); return _builds.Peek(); }
        public Build EndBuild() => _builds.Pop();

        public Build GetBuild() => _builds.Peek();
    }
}
