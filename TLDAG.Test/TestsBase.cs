using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Core;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Test
{
    public class TestsBase
    {
        private string? solutionName = null;
        protected string SolutionName => solutionName ??= GetSolutionName();

        private DirectoryInfo? solutionDirectory = null;
        protected DirectoryInfo SolutionDirectory => solutionDirectory ??= GetSolutionDirectory();

        private DirectoryInfo? testOutputDirectory = null;
        protected DirectoryInfo TestOutputDirectory=> testOutputDirectory ??= CreateTestOutputDirectory();

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected DirectoryInfo GetTestDirectory(bool clear)
        {
            StackFrame stackFrame = new(1, true);
            MethodBase method = Contract.State.NotNull(stackFrame.GetMethod(), "Not called from test method");
            Type methodType = Contract.State.NotNull(method.DeclaringType, "Not called from test method");
            string name = methodType.FullName + "." + method.Name;
            string path = TestOutputDirectory.CombineDirectory(name).FullName;

            return Directory.CreateDirectory(path);
        }

        private string GetSolutionName()
        {
            IEnumerable<string> names = GetType().Assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Where(a => a.Key.Equals("SolutionFileName"))
                .Select(a => a.Value).NotNull();

            Contract.State.Condition(names.Any(), "AssemblyMetadataAttribute with key 'SolutionFileName' not found");

            return names.First();
        }

        private DirectoryInfo GetSolutionDirectory()
            => Directories.GetDirectoryOfFileAbove(Env.CurrentDirectory, SolutionName);

        private DirectoryInfo CreateTestOutputDirectory()
            => Directory.CreateDirectory(SolutionDirectory.CombineDirectory("TestOutput").FullName);
    }
}
