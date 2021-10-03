﻿using System;
using System.IO;
using System.Management.Automation;
using TLDAG.PS.Resources;

namespace TLDAG.PS.Commands
{
    [Cmdlet(VerbsCommon.New, "Solution")]
    [OutputType(typeof(FileInfo))]
    public class NewSolutionCommand : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public string Path { get; set; } = "";

        protected override void ProcessRecord()
        {
            FileInfo? file = File.Exists(Path) ? new(Path) : null;

            if (file == null)
            {
                ValidateDirectory();

                file = CreateNewSolutionFile();
            }

            WriteObject(file);

            base.ProcessRecord();
        }

        private FileInfo CreateNewSolutionFile()
        {
            FileInfo file = new(Path);
            string contents = CreateNewSolutionContents();

            File.WriteAllText(file.FullName, contents);

            return file;
        }

        private static string CreateNewSolutionContents()
        {
            string content = Res.SolutionTemplate;
            Guid guid = Guid.NewGuid();

            content = content.Replace("GUID", guid.ToString());

            return content;
        }

        private void ValidateDirectory()
        {
            string path = System.IO.Path.GetFullPath(Path);
            string? directory = System.IO.Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory);
            }
        }
    }
}
