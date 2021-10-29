﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLDAG.Core.Executing;

namespace TLDAG.Core.Tests.Reflection
{
    [TestClass]
    public class ExecutionTests
    {
        [TestMethod]
        public void Test()
        {
            Execution execution = ExecutionBuilder.Create("dotnet")
                .AddArgument("--info")
                .WorkingDirectory(Env.WorkingDirectory)
                .UseShellExecute(false)
                .Build();

            ExecutionResult result = execution.Execute().ThrowOnError();

            Assert.AreEqual(0, result.ExitCode);
            Assert.IsTrue(result.Outputs.Count > 0);
            Assert.AreEqual(0, result.Errors.Count);
        }
    }
}
