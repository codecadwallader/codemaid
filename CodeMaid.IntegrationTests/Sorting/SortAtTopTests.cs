using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Sorting
{
    [TestClass]
    [DeploymentItem(@"Sorting\Data\SortAtTop.cs", "Data")]
    [DeploymentItem(@"Sorting\Data\SortAtTop_Sorted.cs", "Data")]
    public class SortAtTopTests
    {
        #region Setup

        private ProjectItem _projectItem;

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\SortAtTop.cs");
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
        public void SortAtTop_FormatsAsExpected()
        {
            TestOperations.ExecuteCommandAndVerifyResults(RunSort, _projectItem, @"Data\SortAtTop_Sorted.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void SortAtTop_DoesNothingOnSecondPass()
        {
            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunSort, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunSort(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);
            textDocument.Selection.StartOfDocument();
            textDocument.Selection.LineDown(true, 4);

            var sortCommand = new CommandID(PackageGuids.GuidCodeMaidCommandSortLines, PackageIds.CmdIDCodeMaidSortLines);
            TestUtils.ExecuteCommand(sortCommand);
        }

        #endregion Helpers
    }
}