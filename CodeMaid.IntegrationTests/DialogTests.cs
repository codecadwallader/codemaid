using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using System;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests
{
    [TestClass]
    public class DialogTests
    {
        [TestMethod]
        [HostType("VS IDE")]
        public void ShowAboutTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var dialogBoxPurger = new DialogBoxPurger(NativeMethods.IDOK);

                try
                {
                    dialogBoxPurger.Start();

                    var aboutCommand = new CommandID(PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidAbout);
                    TestUtils.ExecuteCommand(aboutCommand);
                }
                finally
                {
                    dialogBoxPurger.WaitForDialogThreadToTerminate();
                }
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ShowConfigurationTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var dialogBoxPurger = new DialogBoxPurger(NativeMethods.IDOK);

                try
                {
                    dialogBoxPurger.Start();

                    var configurationCommand = new CommandID(PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidOptions);
                    TestUtils.ExecuteCommand(configurationCommand);
                }
                finally
                {
                    dialogBoxPurger.WaitForDialogThreadToTerminate();
                }
            }));
        }
    }
}