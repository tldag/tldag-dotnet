using System;
using System.IO;
using System.Management.Automation;
using TLDAG.Commands.Resources;
using TLDAG.Core.IO;

namespace TLDAG.Commands
{
    [Cmdlet(VerbsCommon.New, "Solution")]
    [OutputType(typeof(FileInfo))]
    public class NewSolution : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        protected override void ProcessRecord()
        {
            FileInfo file = new(Path);

            if (!file.Exists)
            {
                Directories.Validate(file);
                CreateNewSolutionFile(file);
            }

            WriteObject(file);

            base.ProcessRecord();
        }

        private static void CreateNewSolutionFile(FileInfo file)
        {
            string content = Res.SolutionTemplate;
            Guid guid = Guid.NewGuid();

            content = content.Replace("GUID", guid.ToString());
            File.WriteAllText(file.FullName, content);
        }
    }
}
