using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TLDAG.Core;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;

namespace TLDAG.Test
{
    public class TestsBase
    {
        private string? solutionName = null;
        protected string SolutionName => solutionName ??= GetSolutionName();

        private DirectoryInfo? solutionDirectory = null;
        protected DirectoryInfo SolutionDirectory => solutionDirectory ??= Env.WorkingDirectory.GetDirectoryOfFileAbove(SolutionName);

        private DirectoryInfo? testOutputDirectory = null;
        protected DirectoryInfo TestOutputDirectory=> testOutputDirectory ??= SolutionDirectory.CombineDirectory("TestOutput").Created();

        private FileInfo? solutionFile = null;
        protected FileInfo SolutionFile => solutionFile ??= SolutionDirectory.Combine(SolutionName);

        private string? configuration = null;
        protected string Configuration => configuration ??= GetType().Assembly.Configuration();

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected DirectoryInfo GetTestDirectory(bool clear)
        {
            StackFrame stackFrame = new(1, true);
            MethodBase method = Contract.State.NotNull(stackFrame.GetMethod(), "Not called from test method");
            Type type = Contract.State.NotNull(method.DeclaringType, "Not called from test method");
            Assembly assembly = type.Assembly;

            string assemblyName = assembly.Name();
            string typeName = type.FullName ?? "Unknown";
            string methodName = method.Name;
            DirectoryInfo directory = TestOutputDirectory.CombineDirectory(assemblyName, Configuration, typeName, methodName).Created();

            return clear ? directory.Clear() : directory;
        }

        private string GetSolutionName()
        {
            IEnumerable<string> names = GetType().Assembly.GetMetadataValues("SolutionFileName");
            Contract.State.Condition(names.Any(), "AssemblyMetadataAttribute with key 'SolutionFileName' not found");

            return names.First();
        }
    }
}
