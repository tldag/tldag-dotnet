using Microsoft.Build.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TLDAG.Build.Logging
{
    public static class MSBuildDataUtils
    {
        public static Dictionary<string, string> StringDictToDict(IDictionary<string, string>? input)
            => input is null ? new() : new(input);

        public static Dictionary<string, string> StringEnumToDict(IEnumerable? input)
        {
            if (input is not IEnumerable<DictionaryEntry> entries) return new();

            Dictionary<string, string> dict = new();

            foreach (DictionaryEntry entry in entries)
            {
                string key = entry.Key.ToString();
                string value = entry.Value?.ToString() ?? "";

                dict.Remove(key);
                dict[key] = value;
            }

            return dict;
        }
    }

    public class MSBuildBuildData
    {
        public Dictionary<string, string> Environment { get; }

        public MSBuildBuildData(BuildStartedEventArgs args)
        {
            Environment = MSBuildDataUtils.StringDictToDict(args.BuildEnvironment);
        }
    }

    public class MSBuildProjectData
    {
        public int Id { get; }
        public string File { get; }
        public Dictionary<string, string> Globals { get; }
        public Dictionary<string, string> Properties { get; }

        public MSBuildProjectData(ProjectStartedEventArgs args)
        {
            Id = args.ProjectId;
            File = args.ProjectFile;
            Globals = MSBuildDataUtils.StringDictToDict(args.GlobalProperties);
            Properties = MSBuildDataUtils.StringEnumToDict(args.Properties);
        }

        public MSBuildProjectData(ProjectEvaluationFinishedEventArgs args)
        {
            Id = -1;
            File = args.ProjectFile;
            Globals = MSBuildDataUtils.StringEnumToDict(args.GlobalProperties);
            Properties = MSBuildDataUtils.StringEnumToDict(args.Properties);
        }

        internal MSBuildProjectData(int id, string file)
        {
            Id = id;
            File = file;
            Globals = new();
            Properties = new();
        }
    }
}
