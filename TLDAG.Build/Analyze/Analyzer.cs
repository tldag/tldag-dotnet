using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using static System.IO.SearchOption;

namespace TLDAG.Build.Analyze
{
    public enum ProjectType
    {
        Invalid = 0,
        CSharp = 1
    }

    public static class ProjectExtensions
    {
        public static ProjectType GetProjectType(this FileInfo file)
        {
            if (".csproj".Equals(file.Extension)) return ProjectType.CSharp;
            return ProjectType.Invalid;
        }

        public static StringSet SupportedProjectExtensions { get; }
            = new(".csproj");

        public static StringSet SupportedProjectPatterns { get; }
            = new(SupportedProjectExtensions.Select(ext => $"*{ext}"));

        private static readonly Dictionary<ProjectType, StringSet> generatedProjectDirectories = new()
        {
            { ProjectType.Invalid, StringSet.OrdinalEmpty },
            { ProjectType.CSharp, new("bin", "obj") }
        };

        public static IEnumerable<string> GeneratedProjectDirectories(ProjectType projectType)
            => generatedProjectDirectories[projectType];
    }

    public class ProjectData
    {
        public FileInfo File { get; }
        public virtual string Name { get => File.NameWithoutExtension(); }
        public DirectoryInfo Directory { get => File.GetDirectory(); }
        public ProjectType Type { get => File.GetProjectType(); }

        public IEnumerable<DirectoryInfo> SourceDirectories
        {
            get
            {
                IEnumerable<string> ignored = ProjectExtensions.GeneratedProjectDirectories(Type);

                IEnumerable<DirectoryInfo> topDirectories = Directory
                    .EnumerateDirectories("*", TopDirectoryOnly)
                    .Where(dir => ignored.Contains(dir.Name));

                return topDirectories
                    .SelectMany(dir => dir.EnumerateDirectories("*", AllDirectories))
                    .Concat(topDirectories)
                    .Append(Directory);
            }
        }

        public IEnumerable<FileInfo> Sources
            { get => SourceDirectories.SelectMany(dir => dir.EnumerateFiles("*", TopDirectoryOnly)); }

        public ProjectData(FileInfo file) { File = file; }

        public static ProjectData Create(FileInfo file) => new(file);
    }

    public class SolutionProjectData : ProjectData
    {
        public ProjectInSolution Project { get; }

        public override string Name => Project.ProjectName;

        public SolutionProjectData(ProjectInSolution project) : base(new(project.AbsolutePath))
        {
            Project = project;
        }

        public static SolutionProjectData Create(ProjectInSolution project) => new(project);
    }

    public class SolutionData
    {
        public FileInfo File { get; }
        public DirectoryInfo Directory { get => File.GetDirectory(); }
        public SolutionFile SolutionFile { get; }
        public SolutionId SolutionId { get; }
        public SolutionInfo SolutionInfo { get; }

        private List<ProjectData> projects;
        public IReadOnlyList<ProjectData> Projects { get => projects; }

        public SolutionData(FileInfo file, SolutionFile solutionFile, IEnumerable<ProjectData> projects)
        {
            File = file;
            SolutionFile = solutionFile;
            SolutionId = SolutionId.CreateNewId();
            SolutionInfo = SolutionInfo.Create(SolutionId, VersionStamp.Default, File.FullName);
            this.projects = new(projects);
        }
    }

    public class SolutionAnalyzer
    {
        public FileInfo File { get; }
        public DirectoryInfo Directory { get => File.GetDirectory(); }

        public SolutionAnalyzer(FileInfo file)
        {
            File = file;
        }

        public SolutionData Analyze()
        {
            SolutionFile solutionFile = SolutionFile.Parse(File.FullName);

            List<SolutionProjectData> projects = solutionFile.ProjectsInOrder
                .Select(SolutionProjectData.Create)
                .ToList();

            return new(File, solutionFile, projects);
        }

        public static SolutionData Analyze(FileInfo file)
            => new SolutionAnalyzer(file).Analyze();
    }
}
