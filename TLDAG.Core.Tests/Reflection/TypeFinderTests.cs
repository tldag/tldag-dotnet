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
            IEnumerable<TypeDerivatives> deriveds = TypeHierarchy.FindDerivatives(cSharp, true, assembly);
            List<string> lines1 = new();
            List<string> lines2 = new();

            foreach (TypeDerivatives derived in deriveds.OrderBy(d => d))
            {
                lines2.Add(string.Empty);
                lines2.Add($"// {derived.BaseType.FullName}");

                foreach (TypeInfo type in derived.Derivatives.OrderBy(t => t.GetFullName()))
                {
                    string comment = type.IsSealed ? " // sealed" : string.Empty;

                    lines1.Add($"{derived.BaseType.Name} <- {type.FullName}");
                    lines2.Add($"value += nodes.OfType<{type.Name}>().Count();{comment}");
                }
            }

            DirectoryInfo directory = GetTestDirectory();

            directory.Combine("CSharpSyntaxNode1.txt").WriteAllLines(lines1);
            directory.Combine("CSharpSyntaxNode2.txt").WriteAllLines(lines2);
        }
    }
}
