using System.Globalization;
using System.IO;
using System.Text;
using TLDAG.Core.IO;
using static TLDAG.Core.Strings;

namespace TLDAG.Build.Valuating
{
    public partial class ValuationReport
    {
        public void SaveToMarkdown(string prologue, string epilogue, FileInfo output)
        {
            StringBuilder builder = new();

            builder.Append(prologue).Append(NewLine);
            AppendMarkdownSummary(builder);
            AppendMarkdownDetails(builder);
            builder.Append(epilogue);

            output.WriteAllText(builder.ToString(), Encoding.UTF8);
        }

        private void AppendMarkdownSummary(StringBuilder builder)
        {
            string rate = string.Format(ValueFormat, "{0:C0} {1}/Statement.", Factor, Currency);

            builder.Append("## Summary").Append(NewLine).Append(NewLine);

            builder.Append($"Value based on {rate}: ")
                .Append(string.Format(ValueFormat, "**{0:C0} {1}**", Value, Currency))
                .Append(NewLine).Append(NewLine);

            builder.Append("Language | Statements | Value").Append(NewLine);
            builder.Append("--- | ---: | ---:").Append(NewLine);

            foreach (Language language in Languages)
            {
                builder.Append(language.Name).Append(" | ");
                builder.Append(string.Format(ValueFormat, "{0:N0}", language.Statements)).Append(" | ");
                builder.Append(string.Format(ValueFormat, "{0:C0} {1}", language.Value, Currency)).Append(NewLine);
            }

            builder.Append("Total | ");
            builder.Append(string.Format(ValueFormat, "{0:N0}", Statements)).Append(" | ");
            builder.Append(string.Format(ValueFormat, "{0:C0} {1}", Value, Currency)).Append(NewLine);
        }

        private void AppendMarkdownDetails(StringBuilder builder)
        {
            builder.Append("## Details").Append(NewLine);
            builder.Append("Project/Language | Statements | Value").Append(NewLine);
            builder.Append("--- | ---: | ---:").Append(NewLine);

            foreach (Project project in Projects)
            {
                builder.Append($"**{project.Name}**").Append(" | ");
                builder.Append(string.Format(ValueFormat, "{0:N0}", project.Statements)).Append(" | ");
                builder.Append(string.Format(ValueFormat, "{0:C0} {1}", project.Value, Currency)).Append(NewLine);

                foreach (Language language in project.Languages)
                {
                    builder.Append(language.Name).Append(" | ");
                    builder.Append(string.Format(ValueFormat, "{0:N0}", language.Statements)).Append(" | ");
                    builder.Append(string.Format(ValueFormat, "{0:C0} {1}", language.Value, Currency)).Append(NewLine);
                }
            }
        }

        private static readonly NumberFormatInfo ValueFormat = new()
        {
            CurrencyGroupSeparator = "'",
            CurrencySymbol = "",
            NumberGroupSeparator = "'"
        };
    }
}