using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using static TLDAG.Build.Logging.MSBuildEventModel;

namespace TLDAG.Build.Logging
{
    public class MSBuildEventReceiver : IDisposable
    {
        private readonly AnonymousPipeServerStream pipe;
        private readonly MSBuildEventStream stream;

        public MSBuildEventReceiver()
        {
            pipe = new(PipeDirection.In, HandleInheritability.Inheritable);
            stream = new(pipe);
            stream.BeginRead();
        }

        ~MSBuildEventReceiver() { Dispose(false); }
        public void Dispose() { GC.SuppressFinalize(this); Dispose(true); }
        private void Dispose(bool _) { pipe.Dispose(); }

        public string GetSenderLogger()
        {
            Type type = typeof(MSBuildEventSender);

            return $"{type.FullName},\"{type.Assembly.Location}\";{pipe.GetClientHandleAsString()}";
        }

        public BuildResult? GetResult() => JsonConvert.DeserializeObject<BuildResult>(stream.EndRead());
    }
}
