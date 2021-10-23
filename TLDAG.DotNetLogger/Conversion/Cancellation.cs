using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TLDAG.DotNetLogger.Conversion
{
    public static class Cancellation
    {
        private static int ToMillis(TimeSpan? delay)
        {
            if (delay is null) return int.MaxValue;

            return delay.Value.Milliseconds;
        }

        public static CancellationTokenSource CreateCancelSource(TimeSpan? delay) => new(ToMillis(delay));
    }
}
