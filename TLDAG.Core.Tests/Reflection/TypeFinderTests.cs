using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TLDAG.Core.IO;
using TLDAG.Core.Reflection;
using TLDAG.Test;

namespace TLDAG.Core.Tests.Reflection
{
    [TestClass]
    public class TypeFinderTests : TestsBase
    {
        [TestMethod]
        public void FindDerived()
        {
            Type cSharp = typeof(CSharpSyntaxNode);
            Assembly assembly = cSharp.Assembly;
            IEnumerable<IGrouping<Type, TypeInfo>> derived = TypeFinder.FindDerived(cSharp, assembly);
            List<string> lines = new();

            foreach (IGrouping<Type, TypeInfo> group in derived)
            {
                foreach (TypeInfo type in group.OrderBy(t => t.GetFullName()))
                {
                    lines.Add($"{group.Key.Name} <- {type.FullName}");
                }
            }

            GetTestDirectory().Combine("CSharpSyntaxNode.txt").WriteAllLines(lines);
        }
    }
}
