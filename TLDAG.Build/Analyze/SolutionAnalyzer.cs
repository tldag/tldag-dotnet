using Microsoft.Build.Construction;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;

namespace TLDAG.Build.Analyze
{
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
