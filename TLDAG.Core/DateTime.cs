using System;

namespace TLDAG.Core
{
    public static class DateTimeExtensions
    {
        public const string DateTimeDenseStringFormat = "yyyyMMddHHmmssFFF";

        public static string ToDenseString(this DateTime dateTime)
            => dateTime.ToString(DateTimeDenseStringFormat);
    }
}