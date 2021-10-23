using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLDAG.DotNetLogger.IO;

namespace TLDAG.DotNetLogger.Tests.IO
{
    [TestClass]
    public class StringPipeTests
    {
        private static int[] SourceCounts = { 128, 512, 2048 };

        private static List<string> CreateSources()
            => SourceCounts.Select(CreateSource).ToList();

        private static string CreateSource(int count)
        {
            StringBuilder builder = new(count * 13);

            for (int i = 0; i < count; ++i)
                builder.Append("Hello, world!");

            return builder.ToString();
        }

        [TestMethod]
        public void Test()
        {
            using AnonymousPipeServerStream receiverPipe = new(PipeDirection.In);
            using AnonymousPipeClientStream senderPipe = new(PipeDirection.Out, receiverPipe.GetClientHandleAsString());

            List<string> toSend = CreateSources();
            List<int> sentCounts = new();
            List<string> received = new();

            StringPipeReceivedHandler receivedHandler = (_, e) => received.Add(e.Text);
            StringPipeSentHandler sentHandler = (_, e) => sentCounts.Add(e.Count);

            using StringPipeReceiver receiver = new(receiverPipe, receivedHandler);
            StringPipeSender sender = new(senderPipe, receiver.Compressed);

            sender.StringSent += sentHandler;

            foreach (string text in toSend)
                sender.Send(text);

            Task.Delay(10).Wait();

            Assert.AreEqual(toSend.Count, received.Count);

            for (int i = 0; i < toSend.Count; ++i)
            {
                Assert.AreEqual(toSend[i], received[i]);
            }

            sentCounts.ForEach(c => { Debug.WriteLine($"sent {c} bytes"); });
        }
    }
}
