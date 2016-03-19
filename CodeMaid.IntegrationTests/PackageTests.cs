using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests
{
    [TestClass]
    public class PackageTests
    {
        [TestMethod]
        [HostType("VS IDE")]
        public void PackageLoadTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                // Get the Shell Service
                var shellService = VsIdeTestHostContext.ServiceProvider.GetService(typeof(SVsShell)) as IVsShell;
                Assert.IsNotNull(shellService);

                // Validate package load
                IVsPackage package;
                var packageGuid = new Guid(PackageGuids.GuidCodeMaidPackageString);

                Assert.IsTrue(0 == shellService.LoadPackage(ref packageGuid, out package));
                Assert.IsNotNull(package, "Package failed to load");
            }));
        }
    }
}