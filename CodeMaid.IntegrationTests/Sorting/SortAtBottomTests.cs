using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Sorting
{
    [TestClass]
    [DeploymentItem(@"Sorting\Data\SortAtBottom.cs", "Data")]
    [DeploymentItem(@"Sorting\Data\SortAtBottom_Sorted.cs", "Data")]
    public class SortAtBottomTests
    {
        #region Setup

        private ProjectItem _projectItem;

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SortAtBottom.cs");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void SortAtBottom_FormatsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunSort, _projectItem, @"Data\SortAtBottom_Sorted.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void SortAtBottom_DoesNothingOnSecondPass()
        {
            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunSort, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunSort(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);
            textDocument.Selection.EndOfDocument();
            textDocument.Selection.LineUp(true, 4);

            var sortCommand = new CommandID(PackageGuids.GuidCodeMaidCommandSortLines, PackageIds.CmdIDCodeMaidSortLines);
            TestUtils.ExecuteCommand(sortCommand);
        }

        #endregion Helpers
    }
}