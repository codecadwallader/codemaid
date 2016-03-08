#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Integration;
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
                var buildProgressToolWindowCommand = new CommandID(GuidList.GuidCodeMaidCommandBuildProgressToolWindow, (int)PkgCmdIDList.CmdIDCodeMaidBuildProgressToolWindow);
                TestUtils.ExecuteCommand(buildProgressToolWindowCommand);

                Assert.IsTrue(TestUtils.CanFindToolwindow(GuidList.GuidCodeMaidToolWindowBuildProgress));
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ShowSpadeTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var spadeToolWindowCommand = new CommandID(GuidList.GuidCodeMaidCommandSpadeToolWindow, (int)PkgCmdIDList.CmdIDCodeMaidSpadeToolWindow);
                TestUtils.ExecuteCommand(spadeToolWindowCommand);

                Assert.IsTrue(TestUtils.CanFindToolwindow(GuidList.GuidCodeMaidToolWindowSpade));
            }));
        }
    }
}