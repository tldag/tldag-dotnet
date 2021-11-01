using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Build.Analyze;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using static System.IO.SearchOption;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public enum InputType
        {
            Invalid = 0,
            Directory = 1,
            Solution = 2,
            Project = 3
        }

        public static readonly StringSet SupportedProjectExtensions = new(".csproj");
        public static readonly StringSet SupportedProjectPatterns = new(SupportedProjectExtensions.Select(ext => $"*{ext}"));

        public static InputType GetInputType(string path)
        {
            if (path.EndsWith(".sln")) return InputType.Solution;
            if (SupportedProjectExtensions.Contains(Path.GetExtension(path))) return InputType.Project;
            if (Directory.Exists(path)) return InputType.Directory;
            return InputType.Invalid;
        }

        public enum ProjectType
        {
            Invalid = 0,
            CsProject = 1
        }

        public static ProjectType GetProjectType(FileInfo file)
        {
            if (".csproj".Equals(file.Extension)) return ProjectType.CsProject;
            return ProjectType.Invalid;
        }

        public class ProjectInfo
        {
            public FileInfo File { get; }
            public string Name { get => File.NameWithoutExtension(); }
            public ProjectType Type { get; }

            public ProjectInfo(FileInfo file, ProjectType type) { File = file; Type = type; }
        }

        public static ProjectInfo CreateProjectInfo(FileInfo file)
            => new(file, GetProjectType(file));

        public static IEnumerable<ProjectInfo> GetProjects(string path)
        {
            switch (GetInputType(path))
            {
                case InputType.Directory: return GetDirectoryProjects(new(path));
                case InputType.Solution: return GetSolutionProjects(new(path));
                case InputType.Project: return new ProjectInfo[] { CreateProjectInfo(new(path)) };
                default: return Array.Empty<ProjectInfo>();
            }
        }

        private static IEnumerable<ProjectInfo> GetDirectoryProjects(DirectoryInfo directory)
            => directory.EnumerateFiles(SupportedProjectPatterns, AllDirectories).Select(CreateProjectInfo);

        private static IEnumerable<ProjectInfo> GetSolutionProjects(FileInfo file)
            => SolutionAnalyzer.Analyze(file).Projects.Select(p => CreateProjectInfo(p.File)).Where(p => p.Type != ProjectType.Invalid);
    }
}