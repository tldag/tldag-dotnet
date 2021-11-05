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
            => type.FullName ?? "";
    }

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
            => baseType is null || baseType.Equals(type.BaseType);

        public static IEnumerable<TypeInfo> FindDerivedFlat(Type type, IEnumerable<Assembly> assemblies)
        {
            List<TypeInfo> types = new();
            IEnumerable<TypeInfo> candidates = Create(assemblies).BaseType(type).Find();

            while (candidates.Any())
            {
                types.AddRange(candidates);
                candidates = candidates.SelectMany(c => Create(assemblies).BaseType(c).Find());
            }

            return types;
        }

        public static IEnumerable<TypeInfo> FindDerivedFlat(Type type, params Assembly[] assemblies)
            => FindDerivedFlat(type, assemblies.AsEnumerable());

        public static IEnumerable<IGrouping<Type, TypeInfo>> FindDerived(Type type, IEnumerable<Assembly> assemblies)
            => FindDerivedFlat(type, assemblies).GroupBy(t => t.GetBaseType());

        public static IEnumerable<IGrouping<Type, TypeInfo>> FindDerived(Type type, params Assembly[] assemblies)
            => FindDerived(type, assemblies.AsEnumerable());
    }
}
