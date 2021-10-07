using System;
using System.Management.Automation;

namespace TLDAG.Automation
{
    public static class Errors
    {
        public static ErrorRecord Create(Exception exception)
        {
            return CreateUnspecified(exception);
        }

        private static ErrorRecord CreateUnspecified(Exception exception)
            => new(exception, string.Empty, ErrorCategory.NotSpecified, null);
    }
}
