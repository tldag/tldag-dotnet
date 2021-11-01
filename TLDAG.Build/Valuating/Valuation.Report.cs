using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.Build.Valuating
{
    [XmlRoot("report")]
    public partial class NcssReport
    {
        public class Project : IComparable<Project>
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("ncss")]
            public int Ncss { get; set; }

            [XmlElement("language")]
            public List<Language> Languages { get; set; } = new();

            public Project(string name, int ncss) { Name = name; Ncss = ncss; }
            public Project() : this(string.Empty, 0) { }

            public int CompareTo(Project? other) => Name.CompareTo(other?.Name);
        }

        public class Language : IComparable<Language>
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("ncss")]
            public int Ncss { get; set; }

            public Language(string name, int ncss) { Name = name; Ncss = ncss; }
            public Language() : this(string.Empty, 0) { }

            public int CompareTo(Language? other) => Name.CompareTo(other?.Name);
        }

        [XmlAttribute("ncss")]
        public int Ncss { get; set; }

        [XmlElement("language")]
        public List<Language> Languages { get; set; } = new();

        [XmlElement("project")]
        public List<Project> Projects { get; set; } = new();

        public NcssReport(int ncss) { Ncss = ncss; }
        public NcssReport() : this(0) { }

        public static NcssReport Create(string path)
        {
            IEnumerable<Valuation.Ncss> ncsses = Valuation.GetNcsses(path);
            NcssReport report = new(ncsses.Select(n => n.Count).Sum());

            report.Languages = CreateLanguages(ncsses);
            report.Projects = CreateProjects(ncsses);

            return report;
        }

        private static List<Project> CreateProjects(IEnumerable<Valuation.Ncss> ncsses)
        {
            List<Project> projects = new();

            foreach (IGrouping<string, Valuation.Ncss> group in ncsses.ToLookup(n => n.Source.Project.Name))
            {
                int ncss = group.Select(n => n.Count).Sum();
                Project project = new(group.Key, ncss);

                project.Languages = CreateLanguages(group);

                projects.Add(project);
            }

            projects.Sort();

            return projects;
        }

        private static List<Language> CreateLanguages(IEnumerable<Valuation.Ncss> ncsses)
        {
            List<Language> languages = new();

            foreach (IGrouping<string, Valuation.Ncss> group in ncsses.ToLookup(n => n.Source.Language))
            {
                int ncss = group.Select(n => n.Count).Sum();
                Language language = new(group.Key, ncss);

                languages.Add(language);
            }

            languages.Sort();

            return languages;
        }
    }
}
