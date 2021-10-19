using Microsoft.Build.Construction;
using System.IO;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Build.Analyze
{
    public class ProjectAnalyzer
    {
        public ProjectAnalyzer(DirectoryInfo directory, ProjectInSolution project)
        {

        }

        public ProjectData? Analyze()
        {
            throw NotYetImplemented();
        }

        public static ProjectData? Analyze(DirectoryInfo directory, ProjectInSolution project)
            => new ProjectAnalyzer(directory, project).Analyze();
    }
}
