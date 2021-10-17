using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TLDAG.Core;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Build.Workspace.Internal
{
    internal class SolutionParser
    {
        private static readonly StringComparison Ordinal = StringComparison.Ordinal;
        private const string invalidFileFormat = "Invalid file format";

        private static readonly Regex ProjectRex = new
        (
            "^"
            + "Project\\(\"(?<PROJECTTYPEGUID>.*)\"\\)"
            + "\\s*=\\s*"
            + "\"(?<PROJECTNAME>.*)\""
            + "\\s*,\\s*"
            + "\"(?<RELATIVEPATH>.*)\""
            + "\\s*,\\s*"
            + "\"(?<PROJECTGUID>.*)\""
            + "$"
        );

        private readonly FileInfo solutionFile;

        private IEnumerator<string>? lines = null;
        private bool atEOF = false;

        private Version version = new();
        private readonly List<ProjectInfo> projects = new();

        public SolutionParser(FileInfo solutionFile) { this.solutionFile = solutionFile; }

        public SolutionInfo Parse()
        {
            lines = ReadLines();

            ParseHeader();

            for (string? line = NextLine(); line is not null; line = NextLine())
            {
                if (line.StartsWith("Project(", Ordinal))
                    ParseProject(line);
                else if (line.StartsWith("GlobalSection(NestedProjects)", Ordinal))
                    ParseNestedProjects(line);
                else if (line.StartsWith("GlobalSection(SolutionConfigurationPlatforms)", Ordinal))
                    ParseSolutionConfigurationPlatforms();
                else if (line.StartsWith("GlobalSection(ProjectConfigurationPlatforms)", Ordinal))
                    ParseProjectConfigurationPlatforms();
                else if (line.StartsWith("VisualStudioVersion", Ordinal))
                    ParseVisualStudioVersion(line);
            }

            return new(projects);
        }

        private void ParseProject(string firstLine)
        {
            Match match = ProjectRex.Match(firstLine);

            Contract.State.Condition(match.Success, invalidFileFormat);

            for (string? line = NextLine(); line is not null; line = NextLine())
            {
                if (line == "EndProject") break;

                if (line.StartsWith("ProjectSection(ProjectDependencies)", Ordinal))
                    ParseProjectDependencies(line);
                else if (line.StartsWith("ProjectSection(WebsiteProperties)", Ordinal))
                    ParseWebsiteProperties(line);
            }

            ProjectInfo project = new();

            projects.Add(project);
        }

        private void ParseProjectDependencies(string line)
        {
            throw NotYetImplemented();
        }

        private void ParseWebsiteProperties(string line)
        {
            throw NotYetImplemented();
        }

        private void ParseNestedProjects(string line)
        {
            throw NotYetImplemented();
        }

        private void ParseSolutionConfigurationPlatforms()
        {
            for (string? line = NextLine(); line is not null; line = NextLine())
            {
                if (line == "EndGlobalSection") break;
            }
        }

        private void ParseProjectConfigurationPlatforms()
        {
            for (string? line = NextLine(); line is not null; line = NextLine())
            {
                if (line == "EndGlobalSection") break;
            }
        }

        private void ParseVisualStudioVersion(string line)
        {
            // ignored
        }

        private void ParseHeader()
        {
            const string headerPrefix = "Microsoft Visual Studio Solution File, Format Version ";
            string line = Contract.State.NotNull(NextLine(), invalidFileFormat);

            Contract.State.Condition(line.StartsWith(headerPrefix, Ordinal), invalidFileFormat);
            version = Version.Parse(line.Substring(headerPrefix.Length));
            Contract.State.Condition(version.Major == 12, invalidFileFormat);
        }

        private string? NextLine()
        {
            if (atEOF) return null;
            if (lines is null) return null;
            if (!lines.MoveNext()) { atEOF = true; return null; }
            return lines.Current.Trim();
        }

        private IEnumerator<string> ReadLines()
        {
            return solutionFile.ReadAllText().ToLines()
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Where(line => !line.StartsWith("#"))
                .GetEnumerator();
        }

        public static SolutionParser Create(FileInfo solutionFile) => new(solutionFile);
        public static SolutionInfo Parse(FileInfo solutionFile) => Create(solutionFile).Parse();
        
    }
}
