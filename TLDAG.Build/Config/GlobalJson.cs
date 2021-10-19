using Newtonsoft.Json;
using NuGet.Versioning;
using System;
using System.IO;
using System.Text;
using TLDAG.Core;
using TLDAG.Core.IO;
using static TLDAG.Build.Resources.ConfigResources;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Strings;

namespace TLDAG.Build.Config
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GlobalJson
    {
        public class RollForwardValue : IEquatable<RollForwardValue>
        {
            public const string MinorValue = "minor";
            public const string DisableValue = "disable";

            public string Value { get; }

            private RollForwardValue(string value) { Value = value; }

            public override int GetHashCode() => Value.GetHashCode();
            public override bool Equals(object? obj) => EqualsTo(obj as RollForwardValue);
            public bool Equals(RollForwardValue? other) => EqualsTo(other);
            public override string ToString() => Value;

            private bool EqualsTo(RollForwardValue? other) => Value.Equals(other?.Value, OrdinalComparison);

            public static RollForwardValue Parse(string value)
            {
                if (MinorValue.Equals(value, OrdinalComparison)) return Minor;
                if (DisableValue.Equals(value, OrdinalComparison)) return Disable;
                throw InvalidArgument(nameof(value), InvalidRollForwardFormat.Format(value));
            }

            public static RollForwardValue Minor { get; } = new(MinorValue);
            public static RollForwardValue Disable { get; } = new(DisableValue);

            public static bool operator ==(RollForwardValue a, RollForwardValue b) => a.EqualsTo(b);
            public static bool operator !=(RollForwardValue a, RollForwardValue b) => !a.EqualsTo(b);

            public static implicit operator RollForwardValue(string s) => Parse(s);
            public static implicit operator string(RollForwardValue v) => v.Value;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class SdkSection
        {
            [JsonProperty("version")]
            public SemanticVersion Version { get; set; } = new(0, 0, 0);

            [JsonProperty("rollForward")]
            public RollForwardValue RollForward { get; set; } = RollForwardValue.Disable;
        }

        public const string FileName = "global.json";

        [JsonProperty("sdk")]
        public SdkSection Sdk { get; set; } = new();

        public static GlobalJson Get(DirectoryInfo directory) => Load(directory.GetFileAbove(FileName));

        public static GlobalJson Load(FileInfo file)
        {
            string json = file.ReadAllText(Encoding.UTF8);
            GlobalJson? globalJson = JsonConvert.DeserializeObject<GlobalJson>(json);

            return globalJson ?? throw BadFileFormat(InvalidGlobalJson, file);
        }
    }
}
