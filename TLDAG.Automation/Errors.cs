using System;
using System.Diagnostics;
using System.Management.Automation;

namespace TLDAG.Automation
{
    public static class Errors
    {
        public static ErrorRecord Create(Exception exception)
        {
#if DEBUG
            Debug.WriteLine($"No specific ErrorRecord for '{exception.GetType().FullName}'");
#endif
            return CreateUnspecified(exception);
        }

        private static ErrorRecord CreateUnspecified(Exception exception)
            => new(exception, string.Empty, ErrorCategory.NotSpecified, null);
    }
}
