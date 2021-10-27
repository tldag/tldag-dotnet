using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLDAG.DotNetLogger
{
    public class DnlConfig
    {
        public const string AllowedPropertiesPrefix = "AllowedProperties=";
        public const string AllowedMetadataPrefix = "AllowedMetadata=";

        public string PipeHandle { get; }
        public SortedSet<string> AllowedProperties { get; } = new();
        public SortedSet<string> AllowedMetadata { get; } = new();

        public DnlConfig(string pipeHandle, IEnumerable<string>? allowedProperties = null,
            IEnumerable<string>? allowedMetadata = null)
        {
            PipeHandle = pipeHandle;

            if (allowedProperties is not null) AllowProperties(allowedProperties);
            if (allowedMetadata is not null) AllowMetadata(allowedMetadata);
        }

        public void AllowProperties(IEnumerable<string> allowedProperties)
            { foreach (string name in FilterNames(allowedProperties)) AllowedProperties.Add(name); }

        public void AllowMetadata(IEnumerable<string> allowedMetadata)
            { foreach (string name in FilterNames(allowedMetadata)) AllowedMetadata.Add(name); }

        public string ToLogger()
        {
            StringBuilder sb = new();

            sb.Append(PipeHandle);

            if (AllowedProperties.Any())
            {
                sb.Append(";").Append(AllowedPropertiesPrefix);
                sb.Append(string.Join(",", AllowedProperties));
            }

            if (AllowedMetadata.Any())
            {
                sb.Append(";").Append(AllowedMetadataPrefix);
                sb.Append(string.Join(",", AllowedMetadata));
            }

            return sb.ToString();
        }

        public static DnlConfig Parse(string source)
        {
            string[] parts = source.Split(';');
            string pipeHandle = parts.Length > 0 ? parts[0] : "";
            IEnumerable<string>? allowedProperties = null;
            IEnumerable<string>? allowedMetadata = null;

            for (int i = 1; i < parts.Length; ++i)
            {
                string part = parts[i];

                if (part.StartsWith(AllowedPropertiesPrefix))
                    allowedProperties = ParseNames(part.Substring(AllowedPropertiesPrefix.Length));
                else if (part.StartsWith(AllowedMetadataPrefix))
                    allowedMetadata = ParseNames(part.Substring(AllowedMetadataPrefix.Length));
            }

            return new(pipeHandle, allowedProperties, allowedMetadata);
        }

        private static IEnumerable<string> FilterNames(IEnumerable<string> names)
            => names.Select(name => name.Trim()).Where(name => !string.IsNullOrWhiteSpace(name));

        private static SortedSet<string> ParseNames(string source)
            => new(FilterNames(source.Split(',')));
    }
}
