using Newtonsoft.Json;
using System.IO;
using System.Text;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions;
using static TLDAG.Build.Resources.ConfigResources;

namespace TLDAG.Build.Config
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GlobalJson
    {
        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class SdkSection
        {

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
