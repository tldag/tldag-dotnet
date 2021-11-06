using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TLDAG.Core.Reflection
{
    public class TypeDerivatives : IComparable<TypeDerivatives>
    {
        public Type BaseType { get; }

        private List<TypeInfo> derivatives;
        public IReadOnlyList<TypeInfo> Derivatives { get => derivatives; }

        public TypeDerivatives(Type baseType, IEnumerable<TypeInfo> derivatives)
        {
            BaseType = baseType;
            this.derivatives = new(derivatives);
        }

        public int CompareTo(TypeDerivatives? other)
            => BaseType.GetFullName().CompareTo(other?.BaseType.GetFullName());
    }

    public static class TypeHierarchy
    {
        public static IEnumerable<TypeDerivatives> FindDerivatives(Type baseType, bool deep, params Assembly[] assemblies)
            => FindDerivatives(baseType, deep, assemblies.AsEnumerable());

        public static IEnumerable<TypeDerivatives> FindDerivatives(Type baseType, bool deep, IEnumerable<Assembly> assemblies)
            => CreateTypeDerivatives(FindFindDerivativeCandidates(baseType, deep, assemblies));

        private static IEnumerable<TypeInfo> FindFindDerivativeCandidates(Type baseType, bool deep, IEnumerable<Assembly> assemblies)
        {
            List<TypeInfo> types = new();
            IEnumerable<TypeInfo> candidates = TypeFinder.Create(assemblies).BaseType(baseType).Find();

            while (candidates.Any())
            {
                types.AddRange(candidates);
                candidates = deep ? candidates.SelectMany(c => TypeFinder.Create(assemblies).BaseType(c).Find()) : Array.Empty<TypeInfo>();
            }

            return types;
        }

        private static IEnumerable<TypeDerivatives> CreateTypeDerivatives(IEnumerable<TypeInfo> types)
            => types.GroupBy(t => t.GetBaseType()).Select(CreateTypeDerivatives);

        private static TypeDerivatives CreateTypeDerivatives(IGrouping<Type, TypeInfo> group)
            => new(group.Key, group);
    }
}
