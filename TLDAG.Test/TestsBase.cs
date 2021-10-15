using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        protected DirectoryInfo TestOutputDirectory => testOutputDirectory ??= CreateTestOutputDirectory();

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected DirectoryInfo GetTestDirectory(bool clear)
        {
            throw NotYetImplemented();
        }

        private string GetSolutionName()
        {
            throw NotYetImplemented();
        }

        private DirectoryInfo GetSolutionDirectory()
        {
            throw NotYetImplemented();
        }

        private DirectoryInfo CreateTestOutputDirectory()
        {
            throw NotYetImplemented();
        }
    }
}
