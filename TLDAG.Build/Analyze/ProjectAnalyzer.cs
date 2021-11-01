using Microsoft.Build.Construction;
using System.IO;

namespace TLDAG.Build.Analyze
{
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
}
