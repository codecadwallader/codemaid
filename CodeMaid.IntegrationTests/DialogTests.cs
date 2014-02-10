#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Integration;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;

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

                    var aboutCommand = new CommandID(GuidList.GuidCodeMaidCommandAbout, (int)PkgCmdIDList.CmdIDCodeMaidAbout);
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

                    var configurationCommand = new CommandID(GuidList.GuidCodeMaidCommandConfiguration, (int)PkgCmdIDList.CmdIDCodeMaidConfiguration);
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