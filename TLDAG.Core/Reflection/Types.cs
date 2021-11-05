using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLDAG.Core.Reflection
{
    public class TypeFinder
    {
        private readonly List<Assembly> assemblies = new();
        private Type? baseType = null;

        public TypeFinder(params Assembly[] assemblies) : this(assemblies.AsEnumerable()) { }
        public TypeFinder(IEnumerable<Assembly> assemblies) { this.assemblies.AddRange(assemblies); }

        public static TypeFinder Create(params Assembly[] assemblies) => new(assemblies);
        public static TypeFinder Create(IEnumerable<Assembly> assemblies) => new(assemblies);

        public TypeFinder BaseType(Type? type) { baseType = type; return this; }

        public IEnumerable<TypeInfo> Find()
        {
            return assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(HasBaseType);
        }

        private bool HasBaseType(TypeInfo type)
            => baseType is null || AreTypesEqual(baseType, type.BaseType);

        private static bool AreTypesEqual(Type? a, Type? b)
        {
            if (a is null) return b is null;
            if (b is null) return false;

            string? aName = a.FullName;
            string? bName = b.FullName;

            if (aName is null) return bName is null;
            if (bName is null) return false;

            return aName.Equals(bName);
        }
    }
}
