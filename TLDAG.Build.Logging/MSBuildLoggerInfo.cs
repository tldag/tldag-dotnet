using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TLDAG.Build.Logging
{
    public class MSBuildLoggerInfo
    {
        public string Name { get; }
        public FileInfo File { get; }

        private readonly List<string> parameters;
        public IReadOnlyList<string> Parameters { get => parameters; }

        private MSBuildLoggerInfo(string name, FileInfo file, IEnumerable<string> parameters)
        {
            Name = name;
            File = file;

            this.parameters = new(parameters);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append(Name).Append(",").Append($"\"{File.FullName}\"");

            if (parameters.Count > 0)
            {
                sb.Append(";");
                sb.Append(string.Join(",", parameters));
            }

            return sb.ToString();
        }

        public static MSBuildLoggerInfo Create(string fullyQualifiedClassName, FileInfo assemblyFile, params string[] parameters)
            => new(fullyQualifiedClassName, assemblyFile, parameters);

        public static MSBuildLoggerInfo Create<T>(params string[] parameters) where T : ILogger
        {
            string fullyQualifiedClassName = typeof(T).FullName;
            FileInfo assemblyFile = new(typeof(T).Assembly.Location);

            return Create(fullyQualifiedClassName, assemblyFile, parameters);
        }
    }
}
