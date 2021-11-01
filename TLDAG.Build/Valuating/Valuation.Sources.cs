using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using static System.IO.SearchOption;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        public enum SourceType
        {
            Invalid = 0,
            CSharp = 1,
            Xml = 2
        }

        public static SourceType GetSourceType(FileInfo file)
        {
            string extension = file.Extension;

            if (".cs".Equals(extension)) return SourceType.CSharp;
            if (".resx".Equals(extension)) return SourceType.Xml;
            if (".svg".Equals(extension)) return SourceType.Xml;
            if (".csproj".Equals(extension)) return SourceType.Xml;
            if (".props".Equals(extension)) return SourceType.Xml;
            if (".targets".Equals(extension)) return SourceType.Xml;
            return SourceType.Invalid;
        }

        public static string GetSourceLanguage(SourceType type)
        {
            switch (type)
            {
                case SourceType.Invalid: return string.Empty;
                case SourceType.CSharp: return "C#";
                case SourceType.Xml: return "XML";
            }

            return string.Empty;
        }

        public class SourceInfo
        {
            public ProjectInfo Project { get; }
            public FileInfo File { get; }
            public SourceType Type { get; }
            public string Language { get => GetSourceLanguage(Type); }

            public SourceInfo(ProjectInfo project, FileInfo file, SourceType type) { Project = project; File = file; Type = type; }
            public SourceInfo(ProjectInfo project, FileInfo file) : this(project, file, GetSourceType(file)) { }
        }

        public static IEnumerable<SourceInfo> GetSources(string path)
            => GetProjects(path).SelectMany(GetSources);

        public static IEnumerable<SourceInfo> GetSources(ProjectInfo project)
        {
           return GetProjectSourceDirectories(project)
                .SelectMany(dir => dir.EnumerateFiles("*", TopDirectoryOnly))
                .Select(file => new SourceInfo(project, file))
                .Where(source => source.Type != SourceType.Invalid);
        }

        private static IEnumerable<DirectoryInfo> GetProjectSourceDirectories(ProjectInfo project)
        {
            switch (project.Type)
            {
                case ProjectType.CsProject: return GetCsProjectSourceDirectories(project);
                default: return Array.Empty<DirectoryInfo>();
            }
        }

        private static StringSet IgnoredCsProjectDirectories = new("bin", "obj");

        private static IEnumerable<DirectoryInfo> GetCsProjectSourceDirectories(ProjectInfo project)
            => GetProjectSourceDirectories(project, IgnoredCsProjectDirectories);

        private static IEnumerable<DirectoryInfo> GetProjectSourceDirectories(ProjectInfo project, StringSet ignoredDirectories)
        {
            DirectoryInfo directory = project.File.GetDirectory();

            IEnumerable<DirectoryInfo> topDirectories = directory
                .EnumerateDirectories("*", TopDirectoryOnly)
                .Where(dir => !ignoredDirectories.Contains(dir.Name));

            return topDirectories
                .SelectMany(dir => dir.EnumerateDirectories("*", AllDirectories))
                .Concat(topDirectories)
                .Append(directory);
        }
    }
}
