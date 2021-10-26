using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using TLDAG.DotNetLogger.IO;

namespace TLDAG.DotNetLogger.Tests.IO
{
    [TestClass]
    public class BytesPipeTests
    {
        private static int[] SourceSizes = { 1024, 4096, 32768 };

        private static List<byte[]> CreateSources()
            => SourceSizes.Select(CreateSource).ToList();

        private static byte[] CreateSource(int length)
        {
            byte[] source = new byte[length];
            byte b = 0;

            for (int i = 0; i < source.Length; ++i, ++b)
                source[i] = b;

            return source;
        }

        [TestMethod]
        public void Test()
        {
            using AnonymousPipeServerStream receiverPipe = new(PipeDirection.In);
            using AnonymousPipeClientStream senderPipe = new(PipeDirection.Out, receiverPipe.GetClientHandleAsString());

            List<byte[]> toSend = CreateSources();
            List<int> sentCounts = new();
            List<byte[]> received = new();

            BytesPipeReceivedHandler receivedHandler = (_, e) => { received.Add(e.Bytes); };
            BytesPipeSentHandler sentHandler = (_, e) => { sentCounts.Add(e.Count); };

            using BytesPipeReceiver receiver = new(receiverPipe, receivedHandler);
            BytesPipeSender sender = new(senderPipe);

            sender.BytesSent += sentHandler;

            foreach (byte[] bytes in toSend)
                sender.Send(bytes);

            Task.Delay(10).Wait();

            Assert.AreEqual(toSend.Count, received.Count);

            for (int i = 0; i < toSend.Count; ++i)
            {
                Assert.AreEqual(toSend[i].Length, received[i].Length);

                for (int j = 0; j < toSend[i].Length; ++j)
                {
                    Assert.AreEqual(toSend[i][j], received[i][j]);
                }
            }

            sentCounts.ForEach(c => { Debug.WriteLine($"sent {c} bytes"); });
        }
    }
}
