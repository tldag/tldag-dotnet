using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;

namespace TLDAG.DotNetLogger.Model
{
    [Serializable]
    [XmlRoot("log")]
    public class Log
    {
        [XmlAttribute("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [XmlAttribute("transferred")]
        public int Transferred { get; set; }

        [XmlAttribute("success")]
        public bool Success { get; set; } = false;

        [XmlElement("info")]
        public List<string>? Infos { get; set; } = null;

        [XmlElement("project")]
        public List<Project> Projects { get; set; } = new();

        public Project? GetProject(int passId, string? file)
        {
            Project? project = passId < 0 ? null : GetProject(passId);

            if (project is not null)
                return project;

            if (file is null || string.IsNullOrWhiteSpace(file))
                return null;

            project = GetProject(file);

            if (project is null)
                project = AddProject(file);

            project.Passes.Add(passId);

            return project;
        }

        private Project? GetProject(int passId) => Projects.Where(p => p.Passes.Contains(passId)).FirstOrDefault();

        private Project? GetProject(string? file)
            => file is null ? null : Projects.Where(p => file.Equals(p.File, FileNameComparison)).FirstOrDefault();

        private Project AddProject(string file)
        {
            Project project = new(file);

            Projects.Add(project);
            Projects.Sort();

            return project;
        }

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Namespaces { get => namespaces; }

        private static readonly XmlSerializerNamespaces namespaces
            = new(new XmlQualifiedName[] { new("", "urn:log") });
    }
}
