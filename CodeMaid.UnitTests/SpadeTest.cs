#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Integration;
using SteveCadwallader.CodeMaid.UI.ToolWindows.Spade;
using SteveCadwallader.CodeMaid.UnitTests.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    public class SpadeTest
    {
        [TestMethod]
        public void CreateSpadeToolWindow()
        {
            var spadeToolWindow = new SpadeToolWindow();
            Assert.IsNotNull(spadeToolWindow);
        }

        [TestMethod]
        public void ShowSpadeToolWindow()
        {
            var package = CodeMaidPackageHelper.CreateInitializedPackage();

            // Retrieve the command.
            var command = package.MenuCommandService.FindCommand(new CommandID(GuidList.GuidCodeMaidCommandSpadeToolWindow, PackageIds.CmdIDCodeMaidSpadeToolWindow));

            // Invoke the command.
            command.Invoke();
        }
    }
}