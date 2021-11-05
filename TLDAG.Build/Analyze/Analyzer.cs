using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;

namespace TLDAG.Build.Analyze
{
    public class ProjectData
    {
        public ProjectInSolution Project { get; }
        public FileInfo File { get => new(Project.AbsolutePath); }

        public ProjectData(ProjectInSolution project) { Project = project; }
    }

    public class ProjectAnalyzer
    {
        public ProjectInSolution Project { get; }

        public ProjectAnalyzer(DirectoryInfo directory, ProjectInSolution project)
        {
            Project = project;
        }

        public ProjectData? Analyze()
        {
            return new(Project);
        }

        public static ProjectData? Analyze(DirectoryInfo directory, ProjectInSolution project)
            => new ProjectAnalyzer(directory, project).Analyze();
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

            List<ProjectData> projects = solutionFile.ProjectsInOrder
                .Select(project => ProjectAnalyzer.Analyze(Directory, project))
                .NotNull().ToList();

            return new(File, solutionFile, projects);
        }

        public static SolutionData Analyze(FileInfo file)
            => new SolutionAnalyzer(file).Analyze();
    }
}
