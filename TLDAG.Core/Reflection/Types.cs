using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLDAG.Core.Reflection
{
    public static class TypeExtensions
    {
        public static Type GetBaseType(this Type type)
            => Contract.State.NotNull(type.BaseType);

        public static string GetFullName(this Type type)
            => type.FullName ?? type.Name;
    }

    public class TypeFinder
    {
        private readonly List<Assembly> assemblies;
        private Type? baseType = null;

        public TypeFinder(params Assembly[] assemblies) : this(assemblies.AsEnumerable()) { }

        public TypeFinder(IEnumerable<Assembly> assemblies)
            { this.assemblies = new(assemblies.Any() ? assemblies : AppDomain.CurrentDomain.GetAssemblies()); }

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
            => baseType is null || baseType.Equals(type.BaseType);
    }
}
