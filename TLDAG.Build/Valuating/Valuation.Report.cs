using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.Build.Valuating
{
    [XmlRoot("report")]
    public partial class ValuationReport
    {
        public class Project : IComparable<Project>
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("statements")]
            public int Statements { get; set; }

            [XmlAttribute("value")]
            public int Value { get; set; }

            [XmlElement("language")]
            public List<Language> Languages { get; set; } = new();

            public Project(string name, int statements, int value) { Name = name; Statements = statements; Value = value; }
            public Project() : this(string.Empty, 0, 0) { }

            public int CompareTo(Project? other) => Name.CompareTo(other?.Name);
        }

        public class Language : IComparable<Language>
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("statements")]
            public int Statements { get; set; }

            [XmlAttribute("value")]
            public int Value { get; set; }

            public Language(string name, int statements, int value) { Name = name; Statements = statements; Value = value; }
            public Language() : this(string.Empty, 0, 0) { }

            public int CompareTo(Language? other) => Name.CompareTo(other?.Name);
        }

        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlAttribute("factor")]
        public int Factor { get; set; }

        [XmlAttribute("statements")]
        public int Statements { get; set; }

        [XmlAttribute("value")]
        public int Value { get => Factor * Statements; set { } }

        [XmlElement("language")]
        public List<Language> Languages { get; set; } = new();

        [XmlElement("project")]
        public List<Project> Projects { get; set; } = new();

        public ValuationReport(string currency, int factor, int statements)
        {
            Currency = currency;
            Factor = factor;
            Statements = statements;
        }

        public ValuationReport() : this(string.Empty, 0, 0) { }

        public static ValuationReport Create(string path, string currency, int factor)
        {
            IEnumerable<Valuation.SourceValuation> valuations = Valuation.GetValuations(path);
            ValuationReport report = new(currency, factor, valuations.Select(n => n.Count).Sum());

            report.Languages = CreateLanguages(valuations, factor);
            report.Projects = CreateProjects(valuations, factor);

            return report;
        }

        private static List<Project> CreateProjects(IEnumerable<Valuation.SourceValuation> valuations, int factor)
        {
            List<Project> projects = new();

            foreach (IGrouping<string, Valuation.SourceValuation> group in valuations.ToLookup(n => n.Source.Project.Name))
            {
                int statements = group.Select(n => n.Count).Sum();
                Project project = new(group.Key, statements, statements * factor);

                project.Languages = CreateLanguages(group, factor);

                projects.Add(project);
            }

            projects.Sort();

            return projects;
        }

        private static List<Language> CreateLanguages(IEnumerable<Valuation.SourceValuation> valuations, int factor)
        {
            List<Language> languages = new();

            foreach (IGrouping<string, Valuation.SourceValuation> group in valuations.ToLookup(n => n.Source.Language))
            {
                int statements = group.Select(n => n.Count).Sum();

                if (statements > 0)
                    languages.Add(new(group.Key, statements, statements * factor));
            }

            languages.Sort();

            return languages;
        }
    }
}
