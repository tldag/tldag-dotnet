namespace TLDAG.Core
{
    public static class Out
    {
        public static bool Set<T>(T? input, out T output) where T : notnull
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            output = input;
#pragma warning restore CS8601 // Possible null reference assignment.

            return input is not null;
        }
    }
}
