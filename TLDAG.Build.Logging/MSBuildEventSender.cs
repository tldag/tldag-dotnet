using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventSender : Logger
    {
        public override void Initialize(IEventSource eventSource)
        {
            Console.WriteLine("MSBuildEventSender.Initialize");
        }
    }
}
