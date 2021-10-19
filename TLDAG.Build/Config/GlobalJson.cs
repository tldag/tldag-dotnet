using Newtonsoft.Json;
using System.IO;
using System.Text;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Build.Config
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GlobalJson
    {
        public const string FileName = "global.json";

        public static GlobalJson Get(DirectoryInfo directory) => Load(directory.GetFileAbove(FileName));

        public static GlobalJson Load(FileInfo file)
        {
            string json = file.ReadAllText(Encoding.UTF8);

            return JsonConvert.DeserializeObject<GlobalJson>(json);
        }
    }
}
