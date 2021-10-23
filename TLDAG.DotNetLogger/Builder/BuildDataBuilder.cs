using Microsoft.Build.Framework;
using System.Collections.Generic;
using TLDAG.DotNetLogger.Model;
using static TLDAG.DotNetLogger.Algorithm.Algorithms;

namespace TLDAG.DotNetLogger.Builder
{
    public class BuildDataBuilder
    {
        private readonly Dictionary<string, string> environment = new();

        private BuildDataBuilder() { }

        public static BuildDataBuilder Create() => new();

        public BuildDataBuilder Add(BuildStartedEventArgs args)
            { Merge(args.BuildEnvironment, environment); return this; }

        public BuildData Build() { return new(environment); }
    }
}
