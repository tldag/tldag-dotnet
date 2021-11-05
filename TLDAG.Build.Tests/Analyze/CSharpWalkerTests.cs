using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TLDAG.Build.Analyze;
using TLDAG.Core.Collections;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;
using TLDAG.Test;

namespace TLDAG.Build.Tests.Analyze
{
    [TestClass]
    public class CSharpWalkerTests : TestsBase
    {
        [TestMethod]
        public void Test()
        {
            CSharpWalker walker = new();
            SortedSet<string> missing = new();

            walker.UnknownSyntaxNode += (_, e) => { missing.Add(e.Node.GetType().GetFullName()); };

            SolutionAnalyzer.Analyze(SolutionFile).Projects
                .SelectMany(p => p.Sources)
                .Where(f => f.HasExtension(".cs"))
                .Apply(f => walker.Walk(f));

            GetTestDirectory().Combine("missing.txt").WriteAllLines(missing);
        }
    }
}
