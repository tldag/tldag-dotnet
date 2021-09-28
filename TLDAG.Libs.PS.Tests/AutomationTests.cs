using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace TLDAG.Libs.PS.Tests
{
    [TestClass]
    public class AutomationTests
    {
        [TestMethod]
        public void GetPackageSource()
        {
            PowerShell shell = PowerShell.Create();

            shell.AddScript("Get-PackageProvider");
            //shell.AddScript("Get-PackageSource -ProviderName 'PowerShellGet'");

            Collection<PSObject> result = shell.Invoke();
        }
    }
}
