using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TLDAG.Core.Reflection;

namespace TLDAG.Core.Tests.Reflection
{
    [TestClass]
    public class TypeFinderTests
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

            types.ForEach(t => Debug.WriteLine(t.FullName));
        }
    }
}
