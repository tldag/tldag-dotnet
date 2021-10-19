using System;
using static TLDAG.Core.Exceptions.Errors;

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

            public static void Condition(bool value, string paramName, string message)
            {
                if (!value) throw InvalidArgument(paramName, message);
            }

            public static T Min<T>(T value, T min, string paramName) where T : IComparable<T>
            {
                if (value.CompareTo(min) < 0)
                    throw OutOfRange(paramName, value, $"must be at least {min}");

                return value;
            }
        }

        public static class State
        {
            public static void Condition(bool value, string message)
            {
                if (!value) throw InvalidState(message);
            }

            public static T NotNull<T>(T? value, string message)
            {
                if (value is null) throw InvalidState(message);
                return value;
            }
        }
    }
}
