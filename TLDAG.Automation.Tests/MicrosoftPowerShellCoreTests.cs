using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using TLDAG.Test;

namespace TLDAG.Automation.Tests
{
    [TestClass]
    public class MicrosoftPowerShellCoreTests : CommandTests
    {
        protected override Type CommandType => typeof(Command);

        [TestMethod]
        public void FindMicrosoftPowerShellCore()
        {
            CmdletInfo info = Invoke<CmdletInfo>("Get-Command Get-Command");

            Debug.WriteLine(info);
        }
    }
}
