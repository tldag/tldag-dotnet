using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace TLDAG.Tasks
{
    public class Hello : Task
    {
        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Hello 1");
            return true;
        }
    }
}
