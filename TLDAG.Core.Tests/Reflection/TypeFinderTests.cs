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
        public void Test()
        {
            Type cSharp = typeof(CSharpSyntaxNode);
            Assembly assembly = cSharp.Assembly;
            List<TypeInfo> types = new();
            IEnumerable<TypeInfo> candidates;

            candidates = TypeFinder.Create(assembly).BaseType(cSharp).Find();

            while (candidates.Any())
            {
                types.AddRange(candidates.Where(c => !c.IsAbstract));

                candidates = candidates
                    .Where(c => c.IsAbstract)
                    .SelectMany(c => TypeFinder.Create(assembly).BaseType(c).Find());
            }

            FileInfo file = GetTestDirectory(true).Combine("CSharpSyntaxNode.txt");
            IEnumerable<string> names = types.Select(t => t.FullName ?? "").OrderBy(s => s);

            file.WriteAllLines(names);
        }

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

            GetTestDirectory(true).Combine("CSharpSyntaxNode.txt").WriteAllLines(lines);
        }
    }
}
