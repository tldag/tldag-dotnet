using System.IO;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public class SourceInfo
        {
            public Options Options { get => Project.Options; }
            public ProjectInfo Project { get; }
            public FileInfo File { get; }
            public virtual string Language { get; }

            public virtual SourceStatements Statements
                { get => Options.ValuatorFactory.GetValuator(this).Valuate(this); }

            public SourceInfo(ProjectInfo project, FileInfo file, string language)
                { Project = project; File = file; Language = language; }
        }

        public class SourceFactory : Factory
        {
            public SourceFactory(Options options) : base(options) { }

            public virtual SourceInfo GetSource(ProjectInfo project, FileInfo file)
            {
                return file.Extension switch
                {
                    ".cs" => new SourceInfo(project, file, "C#"),
                    ".csproj" => new SourceInfo(project, file, "XML"),
                    ".props" => new SourceInfo(project, file, "XML"),
                    ".resx" => new SourceInfo(project, file, "XML"),
                    ".svg" => new SourceInfo(project, file, "XML"),
                    ".targets" => new SourceInfo(project, file, "XML"),
                    _ => new SourceInfo(project, file, "Other"),
                };
            }
        }
    }
}