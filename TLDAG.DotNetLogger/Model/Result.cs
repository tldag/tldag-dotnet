using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using static TLDAG.DotNetLogger.DotNetLoggerConstants;

namespace TLDAG.DotNetLogger.Model
{
    [XmlRoot("build")]
    public class Result
    {
        private Properties environment = new(EnvNameComparer);

        [XmlElement("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [XmlElement("environment")]
        public Properties Environment
        {
            get => new(environment);
            set { environment = new(value, EnvNameComparer); }
        }

        [XmlElement("project")]
        public List<Project> Projects { get; set; } = new();

        public Project? GetProject(int id, string? file)
        {
            Project? project = GetProject(id);

            if (project is not null)
                return project;

            if (file is null || string.IsNullOrWhiteSpace(file))
                return null;

            project = GetProject(file);

            if (project is null)
                project = AddProject(file);

            project.AddId(id);

            return project;
        }

        private Project? GetProject(int id) => Projects.Where(p => p.HasId(id)).FirstOrDefault();

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
            = new(new XmlQualifiedName[] { new("", "urn:build") });
    }
}
