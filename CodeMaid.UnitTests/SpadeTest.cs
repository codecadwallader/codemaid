using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.UI.ToolWindows.Spade;
using SteveCadwallader.CodeMaid.UnitTests.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.UnitTests
{
    [TestClass]
    [Ignore]
    //TODO: Disabled while experimenting with VS2017 RC.
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
            var command = package.MenuCommandService.FindCommand(new CommandID(PackageGuids.GuidCodeMaidCommandSpadeToolWindow, PackageIds.CmdIDCodeMaidSpadeToolWindow));

            // Invoke the command.
            command.Invoke();
        }
    }
}