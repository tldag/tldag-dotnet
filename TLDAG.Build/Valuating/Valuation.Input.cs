using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Build.Analyze;
using TLDAG.Core.IO;
using static System.IO.SearchOption;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class Input
        {
            public Options Options { get; }
            public virtual IEnumerable<ProjectInfo> Projects { get => Array.Empty<ProjectInfo>(); }
            public virtual IEnumerable<SourceStatements> Statements { get => Projects.SelectMany(p => p.Statements); }

            public Input(Options options) { Options = options; }

            protected virtual ProjectInfo GetProject(FileInfo file)
                => Options.ProjectFactory.GetProject(file);
        }

        public class DirectoryInput : Input
        {
            public DirectoryInfo Directory { get; }

            public override IEnumerable<ProjectInfo> Projects
                => Directory.EnumerateFiles(SupportedProjectPatterns, AllDirectories).Select(GetProject);

            public DirectoryInput(DirectoryInfo directory, Options options) : base(options) { Directory = directory; }
        }

        public class SolutionInput : Input
        {
            public SolutionData Solution { get; }

            public override IEnumerable<ProjectInfo> Projects
                => Solution.Projects.Select(p => GetProject(p.File));

            public SolutionInput(FileInfo file, Options options) : base(options) { Solution = SolutionAnalyzer.Analyze(file); }
        }

        public class ProjectInput : Input
        {
            public FileInfo File { get; }

            public override IEnumerable<ProjectInfo> Projects
                => new ProjectInfo[] { GetProject(File) };

            public ProjectInput(FileInfo file, Options options) : base(options) { File = file; }
        }

        public class InputFactory : Factory
        {
            public InputFactory(Options options) : base(options) { }

            public virtual Input GetInput(string path)
            {
                if (path.EndsWith(".sln"))
                    return new SolutionInput(new(path), Options);

                if (SupportedProjectExtensions.Contains(Path.GetExtension(path)))
                    return new ProjectInput(new(path), Options);

                if (Directory.Exists(path))
                    return new DirectoryInput(new(path), Options);

                return new Input(Options);
            }
        }
    }
}