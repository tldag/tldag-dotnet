using System;
using static TLDAG.Core.Exceptions;

namespace TLDAG.Core
{
    public static class Contract
    {
        public static class Arg
        {
            public static T As<T>(object? arg, string paramName) where T : notnull
            {
                if (arg is null) throw new ArgumentNullException(paramName);

                if (arg is not T result)
                    throw InvalidArgument($"{arg.GetType().FullName} not supported, expected {typeof(T).FullName}", paramName);

                return result;
            }

            public static T Min<T>(T value, T min, string paramName) where T : IComparable<T>
            {
                if (value.CompareTo(min) < 0)
                    throw OutOfRange(paramName, value, $"must be at least {min}");

                return value;
            }
        }
    }
}
