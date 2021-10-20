namespace TLDAG.Build.Logging
{
    public class MSBuildEventReceiver
    {
        public MSBuildLoggerInfo GetSenderInfo()
            => MSBuildLoggerInfo.Create<MSBuildEventSender>();
    }
}
