using System.Globalization;
using System.IO;
using System.Text;
using TLDAG.Core.IO;
using static TLDAG.Core.Strings;

namespace TLDAG.Build.Valuating
{
    public partial class NcssReport
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
            builder.Append("## Summary").Append(NewLine);
            builder.Append("Language | NCSS").Append(NewLine);
            builder.Append("--- | ---:").Append(NewLine);

            foreach (Language language in Languages)
            {
                builder.Append(language.Name).Append(" | ");
                builder.Append(string.Format(NCSSFormat, "{0}", language.Ncss));
                builder.Append(NewLine);
            }

            builder.Append("Total | ");
            builder.Append(string.Format(NCSSFormat, "{0}", Ncss));
            builder.Append(NewLine);
        }

        private void AppendMarkdownDetails(StringBuilder builder)
        {
            builder.Append("## Details").Append(NewLine);
            builder.Append("Project/Language | NCSS").Append(NewLine);
            builder.Append("--- | ---:").Append(NewLine);

            foreach (Project project in Projects)
            {
                builder.Append($"**{project.Name}**").Append(" | ");
                builder.Append(string.Format(NCSSFormat, "{0}", project.Ncss));
                builder.Append(NewLine);

                foreach (Language language in project.Languages)
                {
                    builder.Append(language.Name).Append(" | ");
                    builder.Append(string.Format(NCSSFormat, "{0}", language.Ncss));
                    builder.Append(NewLine);
                }
            }
        }

        private static readonly NumberFormatInfo NCSSFormat = new()
        {
            NumberGroupSeparator = "'"
        };
    }
}