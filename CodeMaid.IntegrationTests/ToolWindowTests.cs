#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SteveCadwallader.CodeMaid.Integration;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;

namespace SteveCadwallader.CodeMaid.IntegrationTests
{
    [TestClass]
    public class ToolWindowTests
    {
        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [HostType("VS IDE")]
        public void ShowBuildProgressTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var testUtils = new TestUtils();

                var buildProgressToolWindowCommand = new CommandID(GuidList.GuidCodeMaidCommandBuildProgressToolWindow, (int)PkgCmdIDList.CmdIDCodeMaidBuildProgressToolWindow);
                testUtils.ExecuteCommand(buildProgressToolWindowCommand);

                Assert.IsTrue(testUtils.CanFindToolwindow(GuidList.GuidCodeMaidToolWindowBuildProgress));
            }));
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void ShowSpadeTest()
        {
            UIThreadInvoker.Invoke(new Action(() =>
            {
                var testUtils = new TestUtils();

                var spadeToolWindowCommand = new CommandID(GuidList.GuidCodeMaidCommandSpadeToolWindow, (int)PkgCmdIDList.CmdIDCodeMaidSpadeToolWindow);
                testUtils.ExecuteCommand(spadeToolWindowCommand);

                Assert.IsTrue(testUtils.CanFindToolwindow(GuidList.GuidCodeMaidToolWindowSpade));
            }));
        }
    }
}