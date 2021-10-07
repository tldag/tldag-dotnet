using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace TLDAG.Commands.Tests
{
    [TestClass]
    public class NewPasswordTests : PowerShellTests
    {
        protected override Type Command => typeof(NewPassword);

        [TestMethod]
        public void MyTestMethod()
        {
            Shell.AddScript("New-Password");

            Collection<string> passwords = Shell.Invoke<string>();

            Assert.AreEqual(1, passwords.Count);
            Assert.AreEqual(24, passwords[0].Length);
        }
    }
}
