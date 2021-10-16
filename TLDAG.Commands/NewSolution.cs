using System;
using System.IO;
using System.Management.Automation;
using TLDAG.Automation;
using TLDAG.Commands.Resources;
using TLDAG.Core.IO;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsCommon.New, "Solution")]
    [OutputType(typeof(FileInfo))]
    public class NewSolution : Command
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        private static void CreateNewSolutionFile(FileInfo file)
        {
            string content = Res.SolutionTemplate;
            Guid guid = Guid.NewGuid();

            content = content.Replace("GUID", guid.ToString());
            File.WriteAllText(file.FullName, content);
        }

        protected override void Process()
        {
            FileInfo file = new(Path);

            if (!file.Exists)
            {
                CreateNewSolutionFile(file);
            }

            WriteObject(new FileInfo(file.FullName));
        }

        protected override void Begin() { }
        protected override void End() { }
    }
}
