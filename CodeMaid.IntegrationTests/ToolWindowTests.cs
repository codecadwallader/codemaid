using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using System;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests
{
    [TestClass]
    public class ToolWindowTests
    {
        [TestMethod]
        [HostType("VS IDE")]
        public void ShowBuildProgressTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var buildProgressToolWindowCommand = new CommandID(PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidBuildProgressToolWindow);
                TestUtils.ExecuteCommand(buildProgressToolWindowCommand);

                Assert.IsTrue(TestUtils.CanFindToolwindow(PackageGuids.GuidCodeMaidToolWindowBuildProgress));
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ShowSpadeTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var spadeToolWindowCommand = new CommandID(PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeToolWindow);
                TestUtils.ExecuteCommand(spadeToolWindowCommand);

                Assert.IsTrue(TestUtils.CanFindToolwindow(PackageGuids.GuidCodeMaidToolWindowSpade));
            }));
        }
    }
}