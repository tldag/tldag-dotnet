using Microsoft.Build.Construction;
using System.IO;

namespace TLDAG.Build.Analyze
{
    public class ProjectData
    {
        public ProjectInSolution Project { get; }
        public FileInfo File { get => new(Project.AbsolutePath); }

        public ProjectData(ProjectInSolution project) { Project = project; }
    }
}
