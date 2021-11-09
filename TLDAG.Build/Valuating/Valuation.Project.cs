using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Build.Analyze;
using TLDAG.Core.IO;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class ProjectInfo
        {
            public FileInfo File { get; }
            public Options Options { get; }

            public string Name { get => File.NameWithoutExtension(); }

            public virtual IEnumerable<DirectoryInfo> SourceDirectories { get => Array.Empty<DirectoryInfo>(); }
            public virtual IEnumerable<SourceInfo> Sources { get => Array.Empty<SourceInfo>(); }
            public virtual IEnumerable<SourceStatements> Statements { get => Sources.Select(s => s.Statements); }

            public ProjectInfo(FileInfo file, Options options) { File = file; Options = options; }

            protected virtual SourceInfo GetSource(FileInfo file)
                => Options.SourceFactory.GetSource(this, file);
        }

        public class CSharpProjectInfo : ProjectInfo
        {
            public ProjectData Data { get; }

            public override IEnumerable<DirectoryInfo> SourceDirectories => Data.SourceDirectories;

            public override IEnumerable<SourceInfo> Sources
                => Data.Sources.Select(GetSource);

            public CSharpProjectInfo(ProjectData data, Options options) : base(data.File, options) { Data = data; }
        }

        public class ProjectFactory : Factory
        {
            public ProjectFactory(Options options) : base(options) { }

            public virtual ProjectInfo GetProject(FileInfo file)
            {
                return file.Extension switch
                {
                    ".csproj" => new CSharpProjectInfo(new(file), Options),
                    _ => new ProjectInfo(file, Options)
                };
            }
        }

    }
}
