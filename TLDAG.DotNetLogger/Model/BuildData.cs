using System.Collections.Generic;

namespace TLDAG.DotNetLogger.Model
{
    public class BuildData
    {
        public Dictionary<string, string> Environment { get; }

        public BuildData(Dictionary<string, string> environment)
        {
            Environment = environment;
        }
    }
}
