using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using TLDAG.Core.IO;

namespace TLDAG.Build.Analyze
{
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
}
