using System;

namespace TLDAG.Core
{
    public static class Contract
    {
        public static T As<T>(object? arg, string name) where T : notnull
        {
            if (arg is null) throw new ArgumentNullException(name);

            if (arg is not T result)
                throw new ArgumentException($"{arg.GetType().FullName} not supported, expected {typeof(T).FullName}", name);

            return result;
        }

        public static T Min<T>(T value, T min, string name) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) throw new ArgumentOutOfRangeException(name, $"must be at least {min}");
            return value;
        }
    }
}
