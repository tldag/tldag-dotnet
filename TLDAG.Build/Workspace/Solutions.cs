using Microsoft.CodeAnalysis;
using System.IO;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Build.Workspace
{
    public static class Solutions
    {
        public static AdhocWorkspace GetWorkspace(FileInfo solutionFile)
        {
            AdhocWorkspace workspace = new();

            return workspace;
        }
    }
}
