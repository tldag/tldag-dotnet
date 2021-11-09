using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace TLDAG.Build.Valuating
{
    public static partial class Valuation
    {
        [XmlRoot("report")]
        public partial class Report
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

            [XmlNamespaceDeclarations]
            public XmlSerializerNamespaces Namespaces { get => new(new XmlQualifiedName[] { new("", "urn:report") }); }

            [XmlIgnore]
            public Options Options { get; set; }

            [XmlAttribute("created")]
            public DateTime Created { get; set; } = DateTime.UtcNow;

            [XmlAttribute("currency")]
            public string Currency { get; set; }

            [XmlAttribute("multiplier")]
            public int Multiplier { get; set; }

            [XmlAttribute("statements")]
            public int Statements { get; set; }

            [XmlAttribute("value")]
            public int Value { get => Multiplier * Statements; set { } }

            [XmlElement("language")]
            public List<Language> Languages { get; set; } = new();

            [XmlElement("project")]
            public List<Project> Projects { get; set; } = new();

            public Report(Options options, int statements)
            {
                Options = options;
                Currency = options.Currency;
                Multiplier = options.Multiplier;
                Statements = statements;
            }

            public Report() : this(new(), 0) { }

            public virtual void Serialize(FileInfo file)
                => Options.SerializerFactory.GetSerializer(file).Serialize(this, file);
        }

        public class ReportFactory
        {
            public Options Options { get; }

            public ReportFactory(Options options) { Options = options; }
            public static ReportFactory Create(Options options) => new(options);

            public static Report CreateReport(Options options, string path)
                => Create(options).CreateReport(path);

            public Report CreateReport(string path)
            {
                IEnumerable<SourceStatements> statements = GetInput(path).Statements;
                Report report = new(Options, statements.Select(s => s.Statements).Sum());

                return report;
            }

            protected virtual Input GetInput(string path)
                => Options.InputFactory.GetInput(path);
        }
    }
}
