using System;

namespace TLDAG.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ServiceAttribute : Attribute
    {
        public Type Type { get; }

        public ServiceAttribute(Type type) { Type = type; }
    }
}
